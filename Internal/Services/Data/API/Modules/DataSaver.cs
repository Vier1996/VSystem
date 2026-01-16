using Newtonsoft.Json;
using V_Server.ServerExternal.Services.Data.API.Model;

namespace V_Server.ServerExternal.Services.Data.API.Modules;

public interface IDataSaver
{
    public void SaveModelInStorageForServer(DataModelBase modelBase);
    public void SaveModelInStorageForUser(string userToken, DataModelBase modelBase);
}

public class DataSaver : IDataSaver
{
    private readonly IDataPath _dataPath;
    private readonly IDataCrypt _dataCrypt;
    
    public DataSaver(IDataPath dataPath, IDataCrypt dataCrypt)
    {
        _dataPath = dataPath;
        _dataCrypt = dataCrypt;
    }

    public void SaveModelInStorageForServer(DataModelBase modelBase)
    {
        if (modelBase.IsDirtyStatus() == false) return;

        string path = _dataPath.GetServerModelPath(modelBase.GetType());

        SaveModelInStorage(path, modelBase);
    }

    public void SaveModelInStorageForUser(string userToken, DataModelBase modelBase)
    {
        if (modelBase.IsDirtyStatus() == false) return;

        string path = _dataPath.GetUserModelPath(userToken, modelBase.GetType());

        SaveModelInStorage(path, modelBase);
    }
    
    private void SaveModelInStorage(string path, DataModelBase modelBase)
    {
        string serializedData = JsonConvert.SerializeObject(modelBase, Formatting.Indented);
        string cryptedData = string.Empty;
        
        cryptedData = _dataCrypt.Encrypt(serializedData);

        modelBase.SetSerializedData(serializedData);

        try
        {
            File.WriteAllText(path, cryptedData);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving data: " + ex.Message);
        }
    }
}