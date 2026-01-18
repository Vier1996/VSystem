namespace VSystem.Internal.Services.Data.Interfaces;

public interface IDataPathManager
{
    public string GetServerModelPath(Type modelType);
    public string GetUserModelPath(string userToken, Type modelType);
}