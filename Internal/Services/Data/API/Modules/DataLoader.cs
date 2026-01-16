using Newtonsoft.Json;

namespace V_Server.ServerExternal.Services.Data.API.Modules;

public interface IDataLoader
{
    public (TModel, string) LoadDataModelJsonForServer<TModel>(Type modelType) where TModel : new();
    public (TModel, string) LoadDataModelJsonForUser<TModel>(string userToken, Type modelType) where TModel : new();
}

public class DataLoader : IDataLoader
{
    private readonly IDataPath _dataPath;
    private readonly IDataCrypt _dataCrypt;
    
    public DataLoader(IDataPath dataPath, IDataCrypt dataCrypt)
    {
        _dataPath = dataPath;
        _dataCrypt = dataCrypt;
    }

    public (TModel, string) LoadDataModelJsonForServer<TModel>(Type modelType) where TModel : new()
    {
        string path = _dataPath.GetServerModelPath(modelType);
        
        return LoadDataModelJson<TModel>(path, modelType);
    }

    public (TModel, string) LoadDataModelJsonForUser<TModel>(string userToken, Type modelType) where TModel : new()
    {
        string path = _dataPath.GetUserModelPath(userToken, modelType);

        return LoadDataModelJson<TModel>(path, modelType);
    }
    
    private (TModel, string) LoadDataModelJson<TModel>(string path, Type modelType) where TModel : new()
    {
        if (File.Exists(path))
        {
            string cryptData = File.ReadAllText(path);
            string data = _dataCrypt.Decrypt(cryptData);

            TModel? model = default;
            
            try
            {
                model = (TModel) JsonConvert.DeserializeObject(data, modelType)!;
            }
            catch (Exception e)
            {
                data = "";
                model = (TModel) Activator.CreateInstance(modelType)!;
            }
                
            return (model, data);
        }
            
        return ((TModel) Activator.CreateInstance(modelType)!, "");
    }
}