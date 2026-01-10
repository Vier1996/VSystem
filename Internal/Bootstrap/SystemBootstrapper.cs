using VSystem.External.ClientSimulator;
using VSystem.Internal.Server;

namespace VSystem.Internal.Bootstrap;

public class SystemBootstrapper : IDisposable
{
    private readonly SystemDependencyBuilder _systemDependencyBuilder;
    private readonly CancellationTokenSource _cancellation; 

    private SystemServer _systemServer;
    private PuppetClient _puppetClient;
    
    public SystemBootstrapper(CancellationTokenSource cancellation)
    {
        _systemDependencyBuilder = new SystemDependencyBuilder();
        _cancellation = cancellation;
    }
    
    public void Dispose()
    {
        _systemDependencyBuilder?.Dispose();
        _systemServer?.Dispose();
        _puppetClient?.Dispose();
    }

    public async Task Run(string[] args)
    {
        await _systemDependencyBuilder.ResolveDependencies();

        if (args is not { Length: > 0 }) throw new ArgumentNullException(nameof(args));
        
        switch (args[0])
        {
            case "server":
            {
                _systemServer = new SystemServer();

                await _systemServer.StartAsync();
                
            } break;
            
            case "client":
            {
                _puppetClient =  new PuppetClient();

                await _puppetClient.RunClient();
                
                await _cancellation.CancelAsync();
                
                _cancellation.Dispose();
            } break;
        }
    }
}