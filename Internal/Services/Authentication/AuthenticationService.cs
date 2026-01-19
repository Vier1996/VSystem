using VSystem.Internal.DataModels.Server.User;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Services.Data;
using VSystem.Internal.Services.Token;

namespace VSystem.Internal.Services.Authentication;

public interface IAuthenticationService : IDisposable
{
    public AuthenticationOperationCallback AuthenticateUser(string login, string password);
}

public class AuthenticationService : IAuthenticationService
{
    private readonly ILoggingService _loggingService;
    private readonly ISecureTokenService _secureTokenService;
    private readonly UsersEntranceModel _usersEntranceModel;
    private readonly object _lockObject;

    public AuthenticationService()
    {
        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out _secureTokenService)
            .Get(out IDataService dataService);
        
        _lockObject = new object();
        _usersEntranceModel = dataService.ResolveServerData<UsersEntranceModel>();
    }
    
    public void Dispose()
    {
        
    }

    public AuthenticationOperationCallback AuthenticateUser(string login, string password)
    {
        lock (_lockObject)
        {
            UserEntranceOperationCallback callback = _usersEntranceModel.FindUserModelByLogin(login);

            if (callback.IsSuccess == false)
            {
                return new AuthenticationOperationCallback()
                {
                    IsSuccess = false,
                    CallbackMessage = "User with this login not registered.",
                    SecureToken = string.Empty
                };
            }

            if (callback.UserModel.Password.Equals(password) == false)
            {
                return new AuthenticationOperationCallback()
                {
                    IsSuccess = false,
                    CallbackMessage = "Wrong user password.",
                    SecureToken = string.Empty
                };
            }

            return new AuthenticationOperationCallback()
            {
                IsSuccess = true,
                CallbackMessage = "OK",
                SecureToken = _secureTokenService.GenerateToken(callback.UserModel.Guid)
            };
        }
    }
}