using VSystem.External.Extensions.UniRx;
using VSystem.Internal.Assembly;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Services.Data.Container;
using VSystem.Internal.Services.Data.Interfaces;
using VSystem.Internal.Services.Data.Model.Server;
using VSystem.Internal.Services.Data.Model.User;
using VSystem.Internal.Services.Data.Modules;

namespace VSystem.Internal.Services.Data;

public record DataServiceInitializeArgs
{
    public int AutoSaveDelay { get; init; }
    public required DataPathManager.DataPathInitializeArgs PathInitializeArgs { get; init; }
}

public class DataService : IDataService
{
    private readonly AssemblyManager _assemblyManager;
    
    private readonly IDataPathManager _pathManager; 
    private readonly IDataModelsManager _modelsManager; 
    private readonly IDataModelContainer _modelContainer;
    
    private readonly List<Type> _serverModelTypes;
    private readonly List<Type> _userModelTypes;
    
    private readonly IDisposable _autoSaveDisposable;
    
    public DataService(DataServiceInitializeArgs initializeArgs)
    {
        AppDependencies.Provider.Get(out _assemblyManager);
        
        _pathManager = new DataPathManager(initializeArgs.PathInitializeArgs);
        _modelsManager = new DataModelsManager(_pathManager);
        
        _serverModelTypes = _assemblyManager.GetTypes<ServerModelAttribute, ServerDataModel>();
        _userModelTypes = _assemblyManager.GetTypes<UserModelAttribute, UserDataModel>();
        _modelContainer = new DataModelContainer(LoadServerDataModels());

        _autoSaveDisposable = UniRxExtension.LoopedTimer(initializeArgs.AutoSaveDelay, initializeArgs.AutoSaveDelay, SaveAllDataModels);
    }
    
    public void Dispose()
    {
        _autoSaveDisposable?.Dispose();
        
        SaveAllDataModels();
    }

    public void SaveAllForce()
    {
        SaveAllDataModels();
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
                
            (ServerDataModel, string) modelData = _modelsManager.LoadServerDataModelFromStorage<ServerDataModel>(modelType);
                
            modelData.Item1.UpdateSerialization(modelData.Item2);
            
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
                
            (UserDataModel, string) modelData = _modelsManager.LoadUserDataModelFromStorage<UserDataModel>(userToken, modelType);
                
            modelData.Item1.UpdateSerialization(modelData.Item2);
            
            models.Add(modelType, modelData.Item1);
        }

        return models;
    }

    private void SaveAllDataModels()
    {
        SaveServerDataModels();
    }
    
    private void SaveServerDataModels()
    {
        foreach (KeyValuePair<Type, ServerDataModel> serverDataKvp in _modelContainer.ServerDataModels)
        {
            _modelsManager.SaveServerDataModelInStorage(serverDataKvp.Value);   
        }
    }
}