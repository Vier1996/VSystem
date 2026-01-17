namespace VSystem.Internal.Services.Data.API.Modules;

public class DataPathManager : IDataPathManager
{
    private readonly DataPathInitializeArgs _initializeArgs;
    
    public DataPathManager(DataPathInitializeArgs initializeArgs)
    {
        _initializeArgs = initializeArgs;
        
        if(Directory.Exists(_initializeArgs.ServerModelDirectoryPath) == false) 
            Directory.CreateDirectory(_initializeArgs.ServerModelDirectoryPath);
        
        if(Directory.Exists(_initializeArgs.UserModelDirectoryPath) == false) 
            Directory.CreateDirectory(_initializeArgs.UserModelDirectoryPath);
    }

    public string GetServerModelPath(Type modelType)
    {
        return $"{_initializeArgs.ServerModelDirectoryPath}/" +
               $"{modelType.Name + _initializeArgs.DataExtension}";
    }
    
    public string GetUserModelPath(string userToken, Type modelType)
    {
        return $"{_initializeArgs.UserModelDirectoryPath}/" +
               $"{userToken}/" +
               $"{modelType.Name + _initializeArgs.DataExtension}";
    }
    
    public record DataPathInitializeArgs
    {
        public required string ServerModelDirectoryPath { get; init; }
        public required string UserModelDirectoryPath { get; init; }
        public required string DataExtension { get; init; }
    }
}