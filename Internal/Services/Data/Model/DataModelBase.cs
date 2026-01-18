using System.Text.Json.Serialization;

namespace VSystem.Internal.Services.Data.Model;

[Serializable]
public abstract record DataModelBase
{
    [JsonIgnore] public bool IsDirty { get; protected set; } = false;
    [JsonIgnore] public string SerializedData { get; private set; } = string.Empty;
    
    public void UpdateSerialization(string serializedData)
    {
        SerializedData = serializedData;
        IsDirty = false;
    }
}