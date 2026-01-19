using System.Net.Sockets;

namespace VSystem.Server.SystemServer.HandleModules.Messages.Interfaces;

public interface IServerMessageProcessor
{
    public Task ProcessMessageAsync(Socket clientSocket, string message);
}