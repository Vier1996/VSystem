namespace VSystem.Internal.Services.Data.API.Modules;

public interface IDataPathManager
{
    public string GetServerModelPath(Type modelType);
    public string GetUserModelPath(string userToken, Type modelType);
}