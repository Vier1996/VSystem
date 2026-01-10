using VSystem.Internal.Bootstrap;

namespace VSystem;

class Program
{
    private static CancellationTokenSource _cancellation; 
    
    public static async Task Main(string[] args)
    {
        _cancellation = new CancellationTokenSource();

        using SystemBootstrapper systemBootstrapper = new SystemBootstrapper(_cancellation);

        try
        {
            await systemBootstrapper.Run(args);
            
            while (_cancellation.IsCancellationRequested == false)
                await Task.Delay(Timeout.Infinite, _cancellation.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException e)
        {
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start system: {ex.Message}");
        }
    }
}