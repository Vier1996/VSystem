namespace VSystem.Internal.Services.ClientToken;

public class ClientTokenService
{
    public Guid GenerateClientToken()
    {
        return Guid.NewGuid();
    }
}