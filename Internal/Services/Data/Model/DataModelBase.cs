using Newtonsoft.Json;

namespace VSystem.Internal.Services.Data.Model;

[Serializable]
public abstract record DataModelBase
{
    [JsonIgnore] public bool IsDirty { get; private set; } = false;
    [JsonIgnore] public string SerializedData { get; private set; } = string.Empty;

    protected void SetModelDirty()
    {
        IsDirty = true;
    }
    
    public void UpdateSerialization(string serializedData)
    {
        SerializedData = serializedData;
        IsDirty = false;
    }
}