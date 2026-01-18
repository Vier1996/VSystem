using System.Net.Sockets;
using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.Clients;

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