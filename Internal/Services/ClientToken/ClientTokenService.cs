namespace V_Server.ServerExternal.Services.ClientToken;

public class ClientTokenService
{
    public Guid GenerateClientToken()
    {
        return Guid.NewGuid();
    }
}