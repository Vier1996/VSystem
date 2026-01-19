using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.API;

public abstract class ServerApiExecutor : IDisposable
{
    public abstract void Dispose();
    public abstract Task<string> Execute(RequestDTO request);
}