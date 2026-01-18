namespace VSystem.Internal.DataModels.Server.Token;

public record SecureTokenInfoModel
{
    public string Token { get; private set; } = string.Empty;
    public Guid UserGuid { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    
    public SecureTokenInfoModel SetToken(string token)
    {
        Token = token;
        return this;
    }
    
    public SecureTokenInfoModel SetUserGuid(Guid guid)
    {
        UserGuid = guid;
        return this;
    }
    
    public SecureTokenInfoModel SetCreatedAt(DateTime createdAt)
    {
        CreatedAt = createdAt;
        return this;
    }
    
    public SecureTokenInfoModel SetExpiresAt(DateTime expiresAt)
    {
        ExpiresAt = expiresAt;
        return this;
    }
}