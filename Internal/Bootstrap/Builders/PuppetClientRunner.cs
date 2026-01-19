using VSystem.External.ClientSimulator;
using VSystem.Internal.Operations;

namespace VSystem.Internal.Bootstrap;

public class PuppetClientRunner : IDependencyBuilder
{
    private PuppetClient _puppetClient;

    public void Dispose()
    {
        _puppetClient?.Dispose();
    }

    public async Task<ServerOperationCallback> Run(CancellationTokenSource appCancellationToken)
    {
        _puppetClient = new PuppetClient();

        try
        {
            await _puppetClient.RunClient();
            await appCancellationToken.CancelAsync();
        }
        catch (Exception e)
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = e.Message
            };
        }
        finally
        {
            appCancellationToken.Dispose();
        }
        
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
}