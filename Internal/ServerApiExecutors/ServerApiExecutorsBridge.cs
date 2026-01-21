using VSystem.Internal.Constants;
using VSystem.Internal.ServerApiExecutors.DataModels;
using VSystem.Internal.ServerApiExecutors.Ping;
using VSystem.Internal.ServerApiExecutors.Register;
using VSystem.Internal.ServerApiExecutors.Time;

namespace VSystem.Internal.ServerApiExecutors;

public class ServerApiExecutorsBridge
{
    private readonly Dictionary<string, Type> _executors = new()
    {
        { AppConstants.ServerAPI.ServerTime, typeof(ServerGettingTimeExecutor) },
        { AppConstants.ServerAPI.ServerPing, typeof(ServerGettingPingExecutor) },
        { AppConstants.ServerAPI.DataModelTestModify, typeof(ModifyTestDataModelExecutor) },
        { AppConstants.ServerAPI.Register.RegisterUser, typeof(RegisterUserExecutor) },
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