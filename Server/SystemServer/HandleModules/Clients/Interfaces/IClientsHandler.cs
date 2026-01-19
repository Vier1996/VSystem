using System.Net.Sockets;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.SystemServer.HandleModules.Clients.Interfaces;

public interface IClientsHandler
{
    public IReadOnlyList<Socket> ConnectedClients { get; }
    
    public Task AddClientAsync(Socket clientSocket);
    public Task RemoveClientAsync(Socket clientSocket);
    public Task BroadcastMessageAsync(string message);
    public Task SendMessageToClientAsync(Socket clientSocket, string responseJson);
    public Task SendMessageToClientAsync(Socket clientSocket, ResponseDTO response);
    public void DisconnectAll();
}