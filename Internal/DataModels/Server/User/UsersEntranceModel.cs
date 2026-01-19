using Newtonsoft.Json;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Data.Model.Server;

namespace VSystem.Internal.DataModels.Server.User;

[Serializable]
public record UsersEntranceModel : ServerDataModel
{
    [JsonIgnore] public IReadOnlyDictionary<Guid, UserEntranceModel> Users => _users;
    
    [JsonProperty] private Dictionary<Guid, UserEntranceModel> _users = new();

    public UserEntranceOperationCallback FindUserModelByLogin(string login)
    {
        (_, UserEntranceModel? userModel) = _users.FirstOrDefault(kvp => kvp.Value.Login.Equals(login));

        if (userModel == null)
        {
            return new UserEntranceOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = $"Can not find user with login: {login}",
                UserModel = null
            };
        }
        
        return new UserEntranceOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK",
            UserModel = userModel
        };
    }
    
    public bool HasUserModel(Guid giud)
    {
        return _users.ContainsKey(giud);
    }
    
    public bool HasUserModel(string login)
    {
        (_, UserEntranceModel? userModel) = _users.FirstOrDefault(kvp => kvp.Value.Login.Equals(login));
        
        return userModel != null;
    }
    
    public ServerOperationCallback AddUserModel(UserEntranceModel? userModel)
    {
        if (userModel == null || userModel.Guid.Equals(Guid.Empty))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user model or guid"
            };
        }
        
        if (HasUserModel(userModel.Guid) || HasUserModel(userModel.Login))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "User with this credentials already exists"
            };
        }
        
        _users[userModel.Guid] = userModel;

        IsDirty = true;
        
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
    
    public ServerOperationCallback AddOrReplaceUserModel(UserEntranceModel? userModel)
    {
        if (userModel == null || userModel.Guid.Equals(Guid.Empty))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user model or guid"
            };
        }
        
        _users[userModel.Guid] = userModel;
        
        IsDirty = true;

        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
    
    public ServerOperationCallback RemoveUserModel(Guid guid)
    {
        if (guid.Equals(Guid.Empty))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user guid"
            };
        }

        if (_users.Remove(guid))
        {
            IsDirty = true;
        }
        
        return new ServerOperationCallback()
        {
            IsSuccess = true,
            CallbackMessage = "OK"
        };
    }
    
    public ServerOperationCallback RemoveUserModel(string login)
    {
        if (string.IsNullOrEmpty(login))
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = "Not valid user login"
            };
        }

        UserEntranceOperationCallback callback = FindUserModelByLogin(login);

        if (callback.IsSuccess == false)
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = $"User with login:[{login}] not exist"
            };
        }
        
        if (_users.Remove(callback.UserModel.Guid))
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