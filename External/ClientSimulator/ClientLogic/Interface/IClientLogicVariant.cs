using System.Net.Sockets;

namespace VSystem.External.ClientSimulator.ClientLogic;

public interface IClientLogicVariant
{
    public Task<string> Run(TcpClient client);
}