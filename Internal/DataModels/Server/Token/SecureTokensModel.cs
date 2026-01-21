using Newtonsoft.Json;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Data.Model.Server;
using VSystem.Internal.Services.Token;

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
    
    public GetTokenOperationCallback GetToken(Guid userGuid)
    {
        if (userGuid.Equals(Guid.Empty))
        {
            return new GetTokenOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user guid",
                TokenInfo = null,
            };
        }

        if (_tokens.TryGetValue(userGuid, out SecureTokenInfoModel tokenInfo) == false)
        {
            return new GetTokenOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Token not exist",
                TokenInfo = null,
            };
        }
        
        return new GetTokenOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK",
            TokenInfo = tokenInfo,
        };
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

        SetModelDirty();
        
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
            SetModelDirty();
        }
       
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
}