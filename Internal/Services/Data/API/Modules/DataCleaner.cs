namespace V_Server.ServerExternal.Services.Data.API.Modules;

public interface IDataCleaner
{
    public void DeleteModelForServer<TModel>(Type modelType) where TModel : new();
    public void DeleteModelForUser<TModel>(string userToken, Type modelType) where TModel : new();
}

public class DataCleaner : IDataCleaner
{
    private readonly IDataPath _dataPath;

    public DataCleaner(IDataPath dataPath)
    {
        _dataPath = dataPath;
    }

    public void DeleteModelForServer<TModel>(Type modelType) where TModel : new()
    {
        string path = _dataPath.GetServerModelPath(modelType);

        DeleteModel(path);
    }

    public void DeleteModelForUser<TModel>(string userToken, Type modelType) where TModel : new()
    {
        string path = _dataPath.GetUserModelPath(userToken, modelType);

        DeleteModel(path);
    }
    
    private void DeleteModel(string path)
    {
        if (!File.Exists(path)) return;
            
        File.Delete(path);
    }
}