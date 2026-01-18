using System.Security.Cryptography;
using System.Text;
using VSystem.External.Extensions.UniRx;
using VSystem.Internal.Constants;
using VSystem.Internal.DataModels.Server.Token;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Data.Interfaces;

namespace VSystem.Internal.Services.Token;

public class SecureTokenService : ISecureTokenService
{
    private readonly ILoggingService _loggingService;
    private readonly SecureTokensModel _secureTokensModel;
    
    private readonly IDisposable _cleanupDisposable;
    private readonly TimeSpan _tokenExpirationTime;
    private readonly object _lockObject;

    public SecureTokenService()
    {
        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out IDataService dataService);

        _tokenExpirationTime = TimeSpan.FromHours(1);
        _lockObject = new object();
        _secureTokensModel = dataService.ResolveServerData<SecureTokensModel>();

        _cleanupDisposable = UniRxExtension.LoopedTimer(
            initialDelay: AppConstants.SecureToken.CleanupInitialSecondsDelay,
            interval: AppConstants.SecureToken.CleanupIntervalCheckSeconds,
            callback: CleanupExpiredTokensAsync);
    }
    
    public void Dispose()
    {
        _cleanupDisposable?.Dispose();
    }

    public string GenerateToken(Guid userGuid)
    {
        lock (_lockObject)
        {
            SecureTokenInfoModel tokenInfo = _secureTokensModel.GetToken(userGuid, out ServerOperationCallback callback);

            if (HasActiveToken(tokenInfo))
            {
                _loggingService.LogMessage(
                    message: $"Returning existing token for user {userGuid}",
                    sender: this);
                
                return tokenInfo.Token;
            }
            
            SecureTokenInfoModel newToken = new SecureTokenInfoModel()
                .SetToken(GenerateSecureToken(userGuid))
                .SetUserGuid(userGuid)
                .SetCreatedAt(DateTime.UtcNow)
                .SetExpiresAt(DateTime.UtcNow.Add(_tokenExpirationTime));
            
            _secureTokensModel.AddToken(userGuid, newToken);

            _loggingService.LogMessage(
                message: $"Generated new token for user {userGuid}. Expires at {newToken.ExpiresAt}",
                sender: this);

            return newToken.Token;
        }
    }
    
    public bool RevokeToken(Guid userGuid)
    {
        lock (_lockObject)
        {
            ServerOperationCallback callback = _secureTokensModel.RemoveToken(userGuid);
            
            if (callback.IsSuccess)
            {
                _loggingService.LogMessage(
                    message: $"Token revoked for user {userGuid}",
                    sender: this);
            }

            return callback.IsSuccess;
        }
    }
    
    public bool HasActiveToken(Guid userGuid)
    {
        lock (_lockObject)
        {
            SecureTokenInfoModel tokenInfo = _secureTokensModel.GetToken(userGuid, out ServerOperationCallback callback);

            return callback.IsSuccess && tokenInfo.ExpiresAt > DateTime.UtcNow;
        }
    }
    
    public bool HasActiveToken(SecureTokenInfoModel tokenInfo)
    {
        lock (_lockObject)
        {
            return tokenInfo.ExpiresAt > DateTime.UtcNow;
        }
    }
    
    private string GenerateSecureToken(Guid userGuid)
    {
        string tokenData = $"{userGuid}:{DateTime.UtcNow.Ticks}:{Guid.NewGuid()}";
        byte[] bytes = Encoding.UTF8.GetBytes(tokenData);
        
        using var sha256 = SHA256.Create();
        byte[] hash = sha256.ComputeHash(bytes);
        
        string base64Hash = Convert.ToBase64String(hash);
        string randomSuffix = Guid.NewGuid().ToString("N")[..16];
        
        return $"{base64Hash}{randomSuffix}";
    }
    
    private void CleanupExpiredTokensAsync()
    {
        while (true)
        {
            try
            {
                lock (_lockObject)
                {
                    List<Guid> expired = _secureTokensModel.Tokens
                        .Where(kvp => kvp.Value.ExpiresAt <= DateTime.UtcNow)
                        .Select(kvp => kvp.Key)
                        .ToList();
                    
                    foreach (Guid expiredUserGuid in expired)
                    {
                        RevokeToken(expiredUserGuid);
                    }

                    if (expired.Any())
                    {
                        _loggingService.LogMessage(
                            message: $"Cleaned up {expired.Count} expired tokens",
                            sender: this);
                    }
                }
            }
            catch (Exception ex)
            {
                _loggingService.LogError(
                    message: $"Error during token cleanup: {ex.Message}",
                    sender: this);
            }
        }
    }
}