namespace VSystem.Internal.Services.Authentication;

public interface IAuthenticationService : IDisposable
{
    public AuthenticationOperationCallback AuthenticateUser(string login, string password);
}