using System.Text.Json.Serialization;

namespace V_Server.ServerExternal.Services.Data.API.Model;

[Serializable]
public abstract class DataModelBase
{
    [JsonIgnore] private bool _isDirty = false;
    [JsonIgnore] private string _serializedData = "";
    
    public void SetSerializedData(string serializedData)
    {
        _serializedData = serializedData;
        _isDirty = false;
    }

    public bool IsDirtyStatus() => _isDirty;
    public void MarkAsDirty() => _isDirty = true;
    public string GetCacheSerializedData() => _serializedData;
}