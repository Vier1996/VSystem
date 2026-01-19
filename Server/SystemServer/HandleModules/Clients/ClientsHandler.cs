using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Server.SystemServer.HandleModules.Clients.Interfaces;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.SystemServer.HandleModules.Clients;

public class ClientsHandler : IClientsHandler
{
    public IReadOnlyList<Socket> ConnectedClients => _clientSockets.ToList().AsReadOnly();
    
    private readonly ConcurrentBag<Socket> _clientSockets = new();

    private readonly ILoggingService _loggingService;
    
    public ClientsHandler()
    {
        AppDependencies.Provider.Get(out _loggingService);
    }
    
    public Task AddClientAsync(Socket clientSocket)
    {
        _clientSockets.Add(clientSocket);
        
        string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
        string message = string.Format(
            format: AppConstants.Server.ClientConnectedMessage,
            arg0: endpoint);

        _loggingService.LogMessage(message: message, sender: this);
        
        return Task.CompletedTask;
    }

    public Task RemoveClientAsync(Socket clientSocket)
    {
        try
        {
            _clientSockets.TryTake(out _);
            
            string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
            string message = string.Format(
                format: AppConstants.Server.ClientDisconnectedMessage,
                arg0: endpoint);

            _loggingService.LogMessage(message: message, sender: this);
        
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
            string message = string.Format(
                format: AppConstants.Server.ClientHandlingError,
                arg0: endpoint,
                arg1: ex.Message);

            _loggingService.LogMessage(message: message, sender: this);
        }

        return Task.CompletedTask;
    }

    public async Task BroadcastMessageAsync(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var disconnectedClients = new List<Socket>();

        foreach (var clientSocket in _clientSockets)
        {
            try
            {
                if (clientSocket.Connected)
                {
                    await clientSocket.SendAsync(messageBytes, SocketFlags.None);
                }
                else
                {
                    disconnectedClients.Add(clientSocket);
                }
            }
            catch (Exception ex)
            {
                disconnectedClients.Add(clientSocket);
                
                string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
                string logMessage = string.Format(
                    format: AppConstants.Server.ClientHandlingError,
                    arg0: endpoint,
                    arg1: ex.Message);

                _loggingService.LogMessage(message: logMessage, sender: this);
            }
        }

        foreach (var client in disconnectedClients)
        {
            await RemoveClientAsync(client);
        }
    }

    public async Task SendMessageToClientAsync(Socket clientSocket, string responseJson)
    {
        try
        {
            if (clientSocket.Connected)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(responseJson);
                
                await clientSocket.SendAsync(messageBytes, SocketFlags.None);
            }
        }
        catch (Exception ex)
        {
            string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
            string message = string.Format(
                format: AppConstants.Server.ClientHandlingError,
                arg0: endpoint,
                arg1: ex.Message);

            _loggingService.LogMessage(message: message, sender: this);
        } 
    }

    public async Task SendMessageToClientAsync(Socket clientSocket, ResponseDTO response)
    {
        try
        {
            if (clientSocket.Connected)
            {
                string message = JsonSerializer.Serialize(response);
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                
                await clientSocket.SendAsync(messageBytes, SocketFlags.None);
            }
        }
        catch (Exception ex)
        {
            string endpoint = clientSocket.RemoteEndPoint?.ToString() ?? AppConstants.Server.ClientEndpointDefaultName;
            string message = string.Format(
                format: AppConstants.Server.ClientHandlingError,
                arg0: endpoint,
                arg1: ex.Message);

            _loggingService.LogMessage(message: message, sender: this);
        }
    }

    public void DisconnectAll()
    {
        foreach (var clientSocket in _clientSockets)
        {
            try
            {
                clientSocket?.Disconnect(false);
                clientSocket?.Close();
                clientSocket?.Dispose();
            }
            catch (Exception ex)
            {          
                string message = string.Format(
                    format: AppConstants.Server.ClientDisconnectingError,
                    arg0: ex.Message);
                
                _loggingService.LogError(message: message, sender: this);
            }
        }
        
        _clientSockets.Clear();
    }
}