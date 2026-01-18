using VSystem.Internal.Services.Data.Model.Server;
using VSystem.Internal.Services.Data.Model.User;

namespace VSystem.Internal.Services.Data.Interfaces;

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