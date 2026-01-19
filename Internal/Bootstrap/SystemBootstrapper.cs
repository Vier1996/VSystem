using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Operations;

namespace VSystem.Internal.Bootstrap;

public class SystemBootstrapper : IDisposable
{
    private readonly List<IDependencyBuilder> _dependencyBuilders;
    private readonly CancellationTokenSource _cancellation; 
    
    public SystemBootstrapper(string[] args, CancellationTokenSource cancellation)
    {
        _cancellation = cancellation;
        _dependencyBuilders = GetDependenciesByAppArgs(args);
    }
    
    public void Dispose()
    {
        AppDependencies.Dispose();
    }

    public async Task Run()
    {
        foreach (IDependencyBuilder builder in _dependencyBuilders)
        {
            ServerOperationCallback callback = await builder.Run(_cancellation);

            if (callback.IsSuccess == false)
            {
                Console.Clear();
                Console.WriteLine(callback.CallbackMessage);
                
                await _cancellation.CancelAsync();

                _cancellation.Dispose();
                
                return;
            }
        }
    }

    private List<IDependencyBuilder> GetDependenciesByAppArgs(string[] args)
    {
        List<IDependencyBuilder> dependencies = new List<IDependencyBuilder>()
        {
            new AppDependenciesBuilder(),
            new ServerDependenciesBuilder(),
        };
        
        if (args is not { Length: > 0 }) 
            throw new ArgumentNullException(nameof(args));
        
        switch (args[0])
        {
            case AppConstants.Assembly.ServerAppArg: 
                dependencies.Add(new ServerRunner());
                break;
            
            case AppConstants.Assembly.ClientAppArg:
                dependencies.Add(new PuppetClientRunner());
                break;
        }
        
        return dependencies;
    }
}