using VSystem.Internal.Bootstrap;

namespace VSystem;

class Program
{
    private static CancellationTokenSource _cancellation; 
    private static SystemBootstrapper? _systemBootstrapper;
    
    public static async Task Main(string[] args)
    { 
        _cancellation = new CancellationTokenSource();
        _systemBootstrapper = new SystemBootstrapper(args, _cancellation);

        SetupExternalProgramCanceling();
        
        try
        {
            await _systemBootstrapper.Run();

            while (_cancellation.IsCancellationRequested == false)
                await Task.Delay(Timeout.Infinite, _cancellation.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException e) { }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start system: {ex.Message}");
        }
        finally
        {
            _systemBootstrapper?.Dispose();
            _cancellation?.Dispose();
        }
    }
    
    private static void SetupExternalProgramCanceling()
    {
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            _cancellation.Cancel();
        };
    }
}