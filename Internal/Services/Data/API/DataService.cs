using V_Server.ServerExternal.Services.Data.API.Container;
using V_Server.ServerExternal.Services.Data.API.Model.Server;
using V_Server.ServerExternal.Services.Data.API.Model.User;
using V_Server.ServerExternal.Services.Data.API.Modules;

namespace V_Server.ServerExternal.Services.Data.API;

public interface IDataService : IDisposable
{
    public void SaveServerDataModels();
    
    public TModel ResolveServerData<TModel>() where TModel : ServerDataModel;
    public TModel ResolveUserData<TModel>(string userToken) where TModel : UserDataModel;
}

public record DataServiceInitializeArgs
{
    public required DataTool.DataToolInitializeArgs ToolInitializeArgs { get; init; }
    public required DataPath.DataPathInitializeArgs PathInitializeArgs { get; init; }
}

public class DataService : IDataService
{
    private readonly DataTool _tool;
    private readonly IDataPath _path;
    private readonly IDataCrypt _crypt;
    private readonly IDataLoader _loader;
    private readonly IDataSaver _saver;
    private readonly IDataCleaner _cleaner;
    private readonly IDataModelContainer _modelContainer;
    
    private readonly List<Type> _serverModelTypes;
    private readonly List<Type> _userModelTypes;
    
    public DataService(DataServiceInitializeArgs initializeArgs)
    {
        _tool = new DataTool(initializeArgs.ToolInitializeArgs);
        _path = new DataPath(initializeArgs.PathInitializeArgs);
        _crypt = new DataCrypt();
        _loader = new DataLoader(_path, _crypt);
        _saver = new DataSaver(_path, _crypt);
        _cleaner = new DataCleaner(_path);
        
        _serverModelTypes = _tool.GetTypes<ServerModelAttribute, ServerDataModel>();
        _userModelTypes = _tool.GetTypes<UserModelAttribute, UserDataModel>();
        _modelContainer = new DataModelContainer(LoadServerDataModels());
    }
    
    public void Dispose()
    {
        SaveServerDataModels();
    }

    public void SaveServerDataModels()
    {
        foreach (KeyValuePair<Type, ServerDataModel> serverDataKvp in _modelContainer.ServerDataModels)
        {
            _saver.SaveModelInStorageForServer(serverDataKvp.Value);   
        }
    }

    public TModel ResolveServerData<TModel>() where TModel : ServerDataModel
    {
        return _modelContainer.ResolveServerData<TModel>();
    }
    
    public TModel ResolveUserData<TModel>(string userToken) where TModel : UserDataModel
    {
        ValidateUserEntryData(userToken);
        
        return _modelContainer.ResolveUserData<TModel>(userToken);
    }

    private void ValidateUserEntryData(string userToken)
    {
        if (_modelContainer.HasUserEntry(userToken) == false)
        {
            _modelContainer.AddUserEntry(userToken);
        }

        if (_modelContainer.IsUserDataLoaded(userToken) == false)
        {
            var userDataModels = LoadUserDataModels(userToken);

            _modelContainer.SetUserDataModels(userToken, userDataModels);
        }
    }
    
    private Dictionary<Type, ServerDataModel> LoadServerDataModels()
    {
        Dictionary<Type, ServerDataModel> models = new Dictionary<Type, ServerDataModel>();

        for (int i = 0; i < _serverModelTypes.Count; i++)
        {
            Type modelType = _serverModelTypes[i];
                
            if(modelType.ContainsGenericParameters || modelType.IsAbstract)
                continue;
                
            (ServerDataModel, string) modelData = _loader.LoadDataModelJsonForServer<ServerDataModel>(modelType);
                
            modelData.Item1.SetSerializedData(modelData.Item2);
            
            models.Add(modelType, modelData.Item1);
        }

        return models;
    }
    
    private Dictionary<Type, UserDataModel> LoadUserDataModels(string userToken)
    {
        Dictionary<Type, UserDataModel> models = new Dictionary<Type, UserDataModel>();

        for (int i = 0; i < _userModelTypes.Count; i++)
        {
            Type modelType = _userModelTypes[i];
                
            if(modelType.ContainsGenericParameters || modelType.IsAbstract)
                continue;
                
            (UserDataModel, string) modelData = _loader.LoadDataModelJsonForUser<UserDataModel>(userToken, modelType);
                
            modelData.Item1.SetSerializedData(modelData.Item2);
            
            models.Add(modelType, modelData.Item1);
        }

        return models;
    }
}