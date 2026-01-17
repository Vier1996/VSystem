using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.Internal.ServerInfrastructure.Server_API_Executors;

public abstract class ServerApiExecutor : IDisposable
{
    public abstract void Dispose();
    public abstract Task<string> Execute(RequestDTO request);
}