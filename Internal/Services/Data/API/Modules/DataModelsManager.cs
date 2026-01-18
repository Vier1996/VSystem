using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Services.Data.API.Model;

namespace VSystem.Internal.Services.Data.API.Modules;

public class DataModelsManager : IDataModelsManager
{
    private readonly ILoggingService _loggingService;
    private readonly IDataPathManager _dataPathManager;
    
    public DataModelsManager(IDataPathManager dataPathManager)
    {
        AppDependencies.Provider.Get(out _loggingService);
        
        _dataPathManager = dataPathManager;
    }
    
    public void SaveServerDataModelInStorage(DataModelBase modelBase)
    {
        if (modelBase.InDirtyStatus() == false) return;

        string path = _dataPathManager.GetServerModelPath(modelBase.GetType());

        SaveDataModelInternal(path, modelBase);
    }

    public void SaveUserDataModelInStorage(string userToken, DataModelBase modelBase)
    {
        if (modelBase.InDirtyStatus() == false) return;

        string path = _dataPathManager.GetUserModelPath(userToken, modelBase.GetType());

        SaveDataModelInternal(path, modelBase);
    }
    
    public (TModel, string) LoadServerDataModelFromStorage<TModel>(Type modelType) where TModel : new()
    {
        string path = _dataPathManager.GetServerModelPath(modelType);
        
        return LoadDataModelInternal<TModel>(path, modelType);
    }

    public (TModel, string) LoadUserDataModelFromStorage<TModel>(string userToken, Type modelType) where TModel : new()
    {
        string path = _dataPathManager.GetUserModelPath(userToken, modelType);

        return LoadDataModelInternal<TModel>(path, modelType);
    }
    
    public void DeleteServerDataModelFromStorage<TModel>(Type modelType) where TModel : new()
    {
        string path = _dataPathManager.GetServerModelPath(modelType);

        DeleteDataModelInternal(path);
    }

    public void DeleteUserDataModelFromStorage<TModel>(string userToken, Type modelType) where TModel : new()
    {
        string path = _dataPathManager.GetUserModelPath(userToken, modelType);

        DeleteDataModelInternal(path);
    }
    
    private void SaveDataModelInternal(string path, DataModelBase modelBase)
    {
        string serializedData = JsonConvert.SerializeObject(modelBase, Formatting.Indented);
       
        modelBase.SetSerializedData(serializedData);

        try
        {
            File.WriteAllText(path, serializedData);
        }
        catch (Exception ex)
        {
            string failureMessage = string.Format(
                format: AppConstants.Data.FailureDataModelSavingMessage,
                arg0: modelBase?.GetType().Name, 
                arg1: path,
                arg2: ex.Message);
            
            _loggingService.LogError(failureMessage, this);
        }
    }
    
    private (TModel, string) LoadDataModelInternal<TModel>(string path, Type modelType) where TModel : new()
    {
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);

            TModel? model = default;
            
            try
            {
                model = (TModel) JsonConvert.DeserializeObject(data, modelType)!;
            }
            catch (Exception e)
            {
                data = string.Empty;
                model = (TModel) Activator.CreateInstance(modelType)!;
            }
            
            return (model, data);
        }
            
        return ((TModel) Activator.CreateInstance(modelType)!, string.Empty);
    }
    
    private void DeleteDataModelInternal(string path)
    {
        if (!File.Exists(path)) return;
            
        File.Delete(path);
    }
}