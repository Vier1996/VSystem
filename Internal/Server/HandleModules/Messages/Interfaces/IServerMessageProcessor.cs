using System.Net.Sockets;

namespace VSystem.Internal.Server.HandleModules.Messages.Interfaces;

public interface IServerMessageProcessor
{
    public Task ProcessMessageAsync(Socket clientSocket, string message);
}