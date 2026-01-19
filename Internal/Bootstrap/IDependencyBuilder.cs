using VSystem.Internal.Operations;

namespace VSystem.Internal.Bootstrap;

public interface IDependencyBuilder : IDisposable
{
    public Task<ServerOperationCallback> Run(CancellationTokenSource appCancellationToken);
}