using System.Net;
using System.Net.Sockets;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.ServerInfrastructure.Server.Configuration;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.Clients.Interfaces;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS.Interfaces;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.Messages.Interfaces;

namespace VSystem.Internal.ServerInfrastructure.Server;

public interface ISystemServer : IDisposable
{
    public Task StartAsync();
    public Task StopAsync();
}

public class SystemServer : ISystemServer
{
    private readonly ServerNetworkSettings _serverNetworkSettings;
    private readonly IClientsHandler _clientsHandler;
    private readonly IServerMessageProcessor _serverMessageProcessor;
    private readonly IDDNSHandler _ddnsHandler;
    private readonly ILoggingService _loggingService;
    
    private bool _isRunning;
    private Socket? _serverSocket;
    private CancellationTokenSource? _ddnsCancellationTokenSource;

    public SystemServer()
    {
        AppDependencies.Provider
            .Get(out _serverNetworkSettings)
            .Get(out _clientsHandler)
            .Get(out _serverMessageProcessor)
            .Get(out _ddnsHandler)
            .Get(out _loggingService);
        
        _isRunning = false;
    }

    public void Dispose()
    {
        _isRunning = false;
        
        StopAsync().Wait();
    }
    
    public async Task StartAsync()
    {
        _loggingService.ClearLogs();
        
        try
        {
            await StartDDNSUpdatesAsync();
            await InitializeServerAsync();
            
            _ = Task.Run(AcceptClientsAsync);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(
                message: string.Format(
                    format: AppConstants.Server.FailedToStartServerErrorMessage,
                    arg0: ex.Message),
                sender: this);

            throw;
        }
    }

    public async Task StopAsync()
    {
        _isRunning = false;

        if (_ddnsCancellationTokenSource?.IsCancellationRequested == false)
        {
            _ddnsCancellationTokenSource?.Cancel();
            _ddnsCancellationTokenSource?.Dispose();
        }

        _clientsHandler.DisconnectAll();
        
        if (_serverSocket != null)
        {
            _serverSocket.Close();
            _serverSocket.Dispose();
            _serverSocket = null;
        }
        
        await Task.CompletedTask;
    }
    
    private async Task InitializeServerAsync()
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _serverNetworkSettings.Port);
        
        _serverSocket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        
        _serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        _serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
    
        if (ipEndPoint.AddressFamily == AddressFamily.InterNetworkV6)
        {
            _serverSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
        }
    
        _serverSocket.Bind(ipEndPoint);
        _serverSocket.Listen(_serverNetworkSettings.MaxConnections);
        
        _isRunning = true;
        
        string externalIp = await _ddnsHandler.GetExternalIpAsync();
        
        _loggingService.LogMessage(
            message: string.Format(
                format: AppConstants.Server.SuccessStartingServerMessage,
                arg0: $"{externalIp}:{_serverNetworkSettings.Port}"),
            sender: this);
    }

    private Task StartDDNSUpdatesAsync()
    {
        _ddnsCancellationTokenSource = new CancellationTokenSource();
        
        _ = Task.Run(() => _ddnsHandler.StartPeriodicUpdatesAsync(_ddnsCancellationTokenSource.Token));
        
        return Task.CompletedTask;
    }

    private async Task AcceptClientsAsync()
    {
        while (_isRunning)
        {
            try
            {
                if (_serverSocket == null) break;
                
                Socket clientSocket = await _serverSocket.AcceptAsync();
                
                await _clientsHandler.AddClientAsync(clientSocket);
                
                _ = Task.Run(() => HandleClientAsync(clientSocket));
            }
            catch (Exception ex)
            {
                if (_isRunning)
                {
                    _loggingService.LogError(
                        message: string.Format(
                            format: AppConstants.Server.FailedToAcceptClientErrorMessage,
                            arg0: ex.Message),
                        sender: this);
                }
            }
        }
    }
    
    private async Task HandleClientAsync(Socket clientSocket)
    {
        var buffer = new byte[_serverNetworkSettings.BufferSize];
        
        try
        {
            while (_isRunning && clientSocket.Connected)
            {
                int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                
                if (received == 0) break;

                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, received);
                
                await _serverMessageProcessor.ProcessMessageAsync(clientSocket, message);
            }
        }
        catch (Exception ex)
        {
            _loggingService.LogError(
                message: string.Format(
                    format: AppConstants.Server.FailedToHandleClientErrorMessage,
                    arg0: clientSocket.RemoteEndPoint,
                    arg1: ex.Message),
                sender: this);
        }
        finally
        {
            await _clientsHandler.RemoveClientAsync(clientSocket);
        }
    }
}