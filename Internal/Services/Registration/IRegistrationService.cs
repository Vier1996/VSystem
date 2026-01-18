using VSystem.Internal.Operations;

namespace VSystem.Internal.Services.Registration;

public interface IRegistrationService : IDisposable
{
    public bool IsRegisteredUser(string login);
    public ServerOperationCallback TryRegisterUser(string login, string password);
    public ServerOperationCallback TryUnregisterUser(Guid guid);
    public ServerOperationCallback TryUnregisterUser(string login);
}