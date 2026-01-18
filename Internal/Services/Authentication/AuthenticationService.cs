using VSystem.Internal.DataModels.Server.User;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Services.Data.Interfaces;

namespace VSystem.Internal.Services.Authentication;

public class AuthenticationService
{
    private readonly ILoggingService _loggingService;
    private readonly UsersEntranceModel _usersEntranceModel;
    
    public AuthenticationService()
    {
        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out IDataService dataService);
    }
}