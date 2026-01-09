using System.Net;
using System.Net.Sockets;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;

namespace VSystem.Internal.Server;

public class ServerRunner
{
    private readonly IClientManager _clientManager;
    private readonly IMessageProcessor _messageProcessor;
    private readonly IDDNSService _ddnsService;
    private readonly ServerNetworkSettings _serverNetworkSettings;
    private readonly ILoggingService _loggingService;
    
    private Socket? _serverSocket;
    private readonly ReactiveProperty<bool> _isRunning = new(false);
    private CancellationTokenSource? _ddnsCancellationTokenSource;

    public ServerRunner()
    {
        AppDependencies.Provider.Get(out _loggingService);
        
        _clientManager = clientManager;
        _messageProcessor = messageProcessor;
        _ddnsService = ddnsService;
        _serverNetworkSettings = serverNetworkSettings;
    }

    public async Task StartAsync()
    {
        LoggingService.Clear();
        
        try
        {
            await InitializeServerAsync();
            await StartDDNSUpdatesAsync();
            
            _ = Task.Run(AcceptClientsAsync);
        }
        catch (Exception ex)
        {
            this.LogError($"Failed to start server: {ex.Message}");
            throw;
        }
    }

    public async Task StopAsync()
    {
        _isRunning.Value = false;

        if (_ddnsCancellationTokenSource?.IsCancellationRequested == false)
        {
            _ddnsCancellationTokenSource?.Cancel();
            _ddnsCancellationTokenSource?.Dispose();
        }

        _clientManager.DisconnectAll();
        
        if (_serverSocket != null)
        {
            _serverSocket.Close();
            _serverSocket.Dispose();
            _serverSocket = null;
        }
        
        await Task.CompletedTask;
    }

    public void Dispose()
    {
        StopAsync().Wait();
        _isRunning?.Dispose();
    }
    
    private async Task InitializeServerAsync()
    {
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _serverNetworkSettings.Port);
        
        _serverSocket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(ipEndPoint);
        _serverSocket.Listen(_serverNetworkSettings.MaxConnections);
        
        _isRunning.Value = true;
        
        string externalIp = await _ddnsService.GetExternalIpAsync();
        
        this.LogMessage($"Server started: [{externalIp}]");
    }

    private async Task StartDDNSUpdatesAsync()
    {
        _ddnsCancellationTokenSource = new CancellationTokenSource();
        _ = Task.Run(() => _ddnsService.StartPeriodicUpdatesAsync(_ddnsCancellationTokenSource.Token));
    }

    private async Task AcceptClientsAsync()
    {
        while (_isRunning.Value)
        {
            try
            {
                if (_serverSocket == null) break;
                
                var clientSocket = await _serverSocket.AcceptAsync();
                await _clientManager.AddClientAsync(clientSocket);
                
                _ = Task.Run(() => HandleClientAsync(clientSocket));
            }
            catch (Exception ex)
            {
                if (_isRunning.Value)
                    this.LogError($"Error accepting client: {ex.Message}");
            }
        }
    }
    
    private async Task HandleClientAsync(Socket clientSocket)
    {
        var buffer = new byte[_serverNetworkSettings.BufferSize];
        
        try
        {
            while (_isRunning.Value && clientSocket.Connected)
            {
                int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                
                if (received == 0) break;

                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, received);
                await _messageProcessor.ProcessMessageAsync(clientSocket, message);
            }
        }
        catch (Exception ex)
        {
            this.LogError($"Client {clientSocket.RemoteEndPoint} error: {ex.Message}");
        }
        finally
        {
            await _clientManager.RemoveClientAsync(clientSocket);
        }
    }
}