using VSystem.Internal.Constants;
using VSystem.Internal.ServerInfrastructure.Server_API_Executors;
using VSystem.Internal.ServerInfrastructure.Server_API_Executors.DataModel;
using VSystem.Internal.ServerInfrastructure.Server_API_Executors.Ping;
using VSystem.Internal.ServerInfrastructure.Server_API_Executors.Time;

namespace VSystem.Internal.ServerInfrastructure.Server;

public class ServerApiExecutorsBridge
{
    private readonly Dictionary<string, Type> _executors = new()
    {
        { AppConstants.ServerAPI.ServerTime, typeof(ServerGettingTimeExecutor) },
        { AppConstants.ServerAPI.ServerPing, typeof(ServerGettingPingExecutor) },
        { AppConstants.ServerAPI.DataModelTestModify, typeof(ModifyTestDataModelExecutor) },
    };
        
    public bool TryGetExecutor(string api, out ServerApiExecutor executor)
    {
        executor = null;
        
        if (_executors.TryGetValue(api, out Type executorType) == false)
            return false;
        
        executor = Activator.CreateInstance(type: executorType) as ServerApiExecutor;

        return executor != null;
    }
}