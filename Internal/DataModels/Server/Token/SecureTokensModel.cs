using Newtonsoft.Json;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Data.Model.Server;

namespace VSystem.Internal.DataModels.Server.Token;

[Serializable]
public record SecureTokensModel : ServerDataModel
{
    [JsonIgnore] public IReadOnlyDictionary<Guid, SecureTokenInfoModel> Tokens => _tokens;
    
    [JsonProperty] private Dictionary<Guid, SecureTokenInfoModel> _tokens = new();
    
    public bool HasSecureToken(Guid userGuid)
    {
        return _tokens.ContainsKey(userGuid);
    }
    
    public SecureTokenInfoModel GetToken(Guid userGuid, out ServerOperationCallback callback)
    {
        if (userGuid.Equals(Guid.Empty))
        {
            callback = new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user guid"
            };
            
            return null;
        }

        if (_tokens.TryGetValue(userGuid, out var tokenInfo) == false)
        {
            callback = new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Token not exist"
            };
        }
        
        callback = new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };

        return tokenInfo;
    }

    public ServerOperationCallback AddToken(Guid userGuid, SecureTokenInfoModel info)
    {
        if (userGuid.Equals(Guid.Empty))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user guid"
            };
        }
        
        if (info == null || string.IsNullOrEmpty(info.Token))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid token info or token value"
            };
        }
        
        if (HasSecureToken(userGuid))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = $"Token:[{info.Token}] already exists"
            };
        }
        
        _tokens[userGuid] = info;

        IsDirty = true;
        
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
    
    public ServerOperationCallback RemoveToken(Guid userGuid)
    {
        if (userGuid.Equals(Guid.Empty))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user guid"
            };
        }
        
        if (_tokens.Remove(userGuid))
        {
            IsDirty = true;
        }
       
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
}