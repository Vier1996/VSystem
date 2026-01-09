using VSystem.Internal.Server.ServerDataBases;

namespace VSystem.Internal.Server_API_Executors;

public abstract class ServerApiExecutor : IDisposable
{
    public abstract void Dispose();
    public abstract Task<string> Execute(RequestBase request);
}