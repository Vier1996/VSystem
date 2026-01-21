using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors;

public abstract class ServerApiExecutor : IDisposable
{
    public abstract void Dispose();
    public abstract Task<ResponseDTO> Execute(RequestDTO request);
}