using VSystem.Internal.Services.Data.Model;

namespace VSystem.Internal.Services.Data;

public interface IDataModelsManager
{
    public void SaveServerDataModelInStorage(DataModelBase modelBase);
    public void SaveUserDataModelInStorage(string userToken, DataModelBase modelBase);

    public (TModel, string) LoadServerDataModelFromStorage<TModel>(Type modelType) where TModel : new();
    public (TModel, string) LoadUserDataModelFromStorage<TModel>(string userToken, Type modelType) where TModel : new();

    public void DeleteServerDataModelFromStorage<TModel>(Type modelType) where TModel : new();
    public void DeleteUserDataModelFromStorage<TModel>(string userToken, Type modelType) where TModel : new();
}