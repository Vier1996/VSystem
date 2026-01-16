namespace V_Server.ServerExternal.Services.Data.API.Modules;

public interface IDataPath
{
    public string GetServerModelPath(Type modelType);
    public string GetUserModelPath(string userToken, Type modelType);
}

public class DataPath : IDataPath
{
    private readonly DataPathInitializeArgs _initializeArgs;
    
    public DataPath(DataPathInitializeArgs initializeArgs)
    {
        _initializeArgs = initializeArgs;
        
        if(Directory.Exists(_initializeArgs.ServerModelDirectoryPath) == false) 
            Directory.CreateDirectory(_initializeArgs.ServerModelDirectoryPath);
        
        if(Directory.Exists(_initializeArgs.UserModelDirectoryPath) == false) 
            Directory.CreateDirectory(_initializeArgs.UserModelDirectoryPath);
    }

    public string GetServerModelPath(Type modelType)
    {
        return Path.Combine(
            path1: _initializeArgs.ServerModelDirectoryPath, 
            path2: modelType.Name + _initializeArgs.DataExtension);
    }
    
    public string GetUserModelPath(string userToken, Type modelType)
    {
        return Path.Combine(
            path1: _initializeArgs.UserModelDirectoryPath, 
            path2: userToken, 
            path3: modelType.Name + _initializeArgs.DataExtension);
    }
    
    public record DataPathInitializeArgs
    {
        public required string ServerModelDirectoryPath { get; init; }
        public required string UserModelDirectoryPath { get; init; }
        public required string DataExtension { get; init; }
    }
}