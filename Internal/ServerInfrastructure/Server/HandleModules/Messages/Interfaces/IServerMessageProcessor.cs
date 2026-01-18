using System.Net.Sockets;

namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.Messages;

public interface IServerMessageProcessor
{
    public Task ProcessMessageAsync(Socket clientSocket, string message);
}