using VSystem.Internal.Services.Data.API.Model.User;

namespace VSystem.Internal.Services.Data.API.Container;

public class DataModelUserContainerEntry
{
    public DateTime LastModifiedTime { get; private set; }
    public bool IsModelsLoaded => _models != null;

    private Dictionary<Type, UserDataModel> _models = null;
    
    public void SetModels(Dictionary<Type, UserDataModel> models)
    {
        _models = models;
    }
    
    public TModel Resolve<TModel>() where TModel : UserDataModel
    {
        Type demandedType = typeof(TModel);

        if (_models.TryGetValue(demandedType, out UserDataModel model))
            return (TModel)model;
            
        throw new ArgumentException($"Model with type of {demandedType} not present in container.");
    }
    
    public bool TryResolve<TModel>(out TModel model) where TModel : UserDataModel
    {
        model = null;

        if (IsModelsLoaded == false)
            return false;

        model = (TModel)_models[typeof(TModel)];

        return true;
    } 
    
    public void UpdateModifiedTime()
    {
        LastModifiedTime = DateTime.UtcNow;
    }
}