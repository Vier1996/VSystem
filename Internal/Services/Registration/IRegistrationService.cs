using VSystem.Internal.Operations;

namespace VSystem.Internal.Services.Registration;

public interface IRegistrationService : IDisposable
{
    public bool IsRegisteredUser(string login);
    public ServerOperationCallback RegisterUser(string login, string password);
    public ServerOperationCallback UnregisterUser(Guid guid);
    public ServerOperationCallback UnregisterUser(string login);
}