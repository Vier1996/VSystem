using System.Collections.Concurrent;
using V_Server.ServerExternal.Services.Data.API.Model.Server;
using V_Server.ServerExternal.Services.Data.API.Model.User;

namespace V_Server.ServerExternal.Services.Data.API.Container;

public interface IDataModelContainer
{
    public IReadOnlyDictionary<Type, ServerDataModel> ServerDataModels { get; }
    
    public TModel ResolveServerData<TModel>() where TModel : ServerDataModel;
    public TModel ResolveUserData<TModel>(string userToken) where TModel : UserDataModel;
    public bool TryResolveUserData<TModel>(string userToken, out TModel model) where TModel : UserDataModel;
    
    public bool HasUserEntry(string userToken);
    public void AddUserEntry(string userToken);
    public bool IsUserDataLoaded(string userToken);
    public void SetUserDataModels(string userToken, Dictionary<Type, UserDataModel> models);
}

public class DataModelContainer : IDataModelContainer
{
    public IReadOnlyDictionary<Type, ServerDataModel> ServerDataModels { get; private set; }
    
    private readonly Dictionary<string, DataModelUserContainerEntry> _users;

    public DataModelContainer(Dictionary<Type, ServerDataModel> serverModels)
    {
        ServerDataModels = new ConcurrentDictionary<Type, ServerDataModel>(serverModels);
        _users = new Dictionary<string, DataModelUserContainerEntry>();
    }
    
    public TModel ResolveServerData<TModel>() where TModel : ServerDataModel
    {
        Type demandedType = typeof(TModel);

        if (ServerDataModels.TryGetValue(demandedType, out var model))
        {
            return (TModel)model;
        }
            
        throw new ArgumentException($"Model with type of {demandedType} not present in container.");
    }

    public TModel ResolveUserData<TModel>(string userToken) where TModel : UserDataModel
    {
        DataModelUserContainerEntry entry = _users[userToken];
        
        return entry.Resolve<TModel>();
    }
    
    public bool TryResolveUserData<TModel>(string userToken, out TModel model) where TModel : UserDataModel
    {
        model = null;
        
        if (_users.TryGetValue(userToken, out DataModelUserContainerEntry entry) == false)
            return false;

        return entry.TryResolve(out model);
    }
    
    public bool HasUserEntry(string userToken)
    {
        return _users.ContainsKey(userToken);
    }

    public void AddUserEntry(string userToken)
    {
        if (_users.ContainsKey(userToken)) return;
        
        _users.Add(userToken, new DataModelUserContainerEntry());
    }

    public bool IsUserDataLoaded(string userToken)
    {
        return _users.TryGetValue(userToken, out var userData) && userData.IsModelsLoaded;
    }

    public void SetUserDataModels(string userToken, Dictionary<Type, UserDataModel> models)
    {
        if (_users.TryGetValue(userToken, out DataModelUserContainerEntry entry) == false)
            return;

        entry.SetModels(models);
    }
}