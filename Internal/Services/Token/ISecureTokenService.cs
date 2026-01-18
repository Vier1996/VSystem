using VSystem.Internal.DataModels.Server.Token;

namespace VSystem.Internal.Services.Token;

public interface ISecureTokenService : IDisposable
{
    public string GenerateToken(Guid userGuid);
    public bool RevokeToken(Guid userGuid);
    public bool HasActiveToken(Guid userGuid);
    public bool HasActiveToken(SecureTokenInfoModel tokenInfo);
}