using VSystem.Client;
using VSystem.Constants;
using VSystem.Server;

namespace VSystem;

class Program
{
    private static CancellationTokenSource _cancellation = null!;

    public static async Task Main(string[] args)
    {
        _cancellation = new CancellationTokenSource();

        SetupExternalProgramCanceling();

        if (IsClientMode(args))
        {
            RestClientRunner clientRunner = new RestClientRunner();
            
            await clientRunner.RunAsync(_cancellation.Token);
            
            return;
        }

        await new ServerRunner().RunAsync(args, _cancellation.Token);
    }

    private static bool IsClientMode(string[] args)
    {
        return args.Length > 0 &&
               args[0].Equals(AssemblyConstants.ClientAppArg, StringComparison.OrdinalIgnoreCase);
    }

    private static void SetupExternalProgramCanceling()
    {
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            _cancellation.Cancel();
        };
    }
}
