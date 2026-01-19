using VSystem.Internal.DataModels.Server.User;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Data;

namespace VSystem.Internal.Services.Registration;

public class RegistrationService : IRegistrationService
{
    private readonly ILoggingService _loggingService;
    private readonly UsersEntranceModel _usersEntranceModel;
    
    public RegistrationService()
    {
        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out IDataService dataService);

        _usersEntranceModel = dataService.ResolveServerData<UsersEntranceModel>();
    }
    
    public void Dispose()
    {
    }
    
    public bool IsRegisteredUser(string login)
    {
        return _usersEntranceModel.HasUserModel(login);
    }
    
    public ServerOperationCallback TryRegisterUser(string login, string password)
    {
        if (_usersEntranceModel.HasUserModel(login))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "User with this credentials already exists"
            };
        }
        
        return _usersEntranceModel.AddUserModel(new UserEntranceModel()
            .SetGuid(Guid.NewGuid())
            .SetLogin(login)
            .SetPassword(password));
    }
    
    public ServerOperationCallback TryUnregisterUser(Guid guid)
    {
        return _usersEntranceModel.RemoveUserModel(guid);
    }
    
    public ServerOperationCallback TryUnregisterUser(string login)
    {
        return _usersEntranceModel.RemoveUserModel(login);
    }
}