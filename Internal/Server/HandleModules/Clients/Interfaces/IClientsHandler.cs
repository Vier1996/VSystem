using System.Net.Sockets;
using VSystem.Internal.Server.ServerDataBases;

namespace VSystem.Internal.Server.HandleModules.Clients.Interfaces;

public interface IClientsHandler
{
    public IReadOnlyList<Socket> ConnectedClients { get; }
    
    public Task AddClientAsync(Socket clientSocket);
    public Task RemoveClientAsync(Socket clientSocket);
    public Task BroadcastMessageAsync(string message);
    public Task SendMessageToClientAsync(Socket clientSocket, string responseJson);
    public Task SendMessageToClientAsync(Socket clientSocket, ResponseBase response);
    public void DisconnectAll();
}