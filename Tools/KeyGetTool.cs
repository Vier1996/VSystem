using Newtonsoft.Json;
using VSystem.Constants;
using VSystem.Internal.Keys;

namespace VSystem.Tools;

public static class KeyGetTool
{
    private static readonly Dictionary<string, IApiKey> _cachedKeys;

    static KeyGetTool()
    {
        _cachedKeys = new Dictionary<string, IApiKey>();
    }
    
    public static T GetKey<T>() where T : class, IApiKey
    {
        string keyName = typeof(T).Name;
        
        if (_cachedKeys.TryGetValue(keyName, out var key)) return (T)key;
            
        string keyPath = GetKeyPath(keyName);

        /*if (File.Exists(keyPath) == false)
        {
            throw new FileNotFoundException(string.Format(AppConstants.Bootstrapper.NotFoundRunEnvironmentKeyErrorMessage, keyPath), keyPath);
        }*/

        string json = File.ReadAllText(keyPath);
        
        var deserializedKey = JsonConvert.DeserializeObject<T>(json);

        /*if (deserializedKey == null)
        {
            throw new InvalidDataException(string.Format(AppConstants.Bootstrapper.InvalidRunEnvironmentKeyErrorMessage, keyPath));
        }*/

        _cachedKeys.Add(keyName, deserializedKey);
        
        return deserializedKey;
    }
    
    private static string GetKeyPath(string keyName)
    {
        return Path.Combine(
            EnvironmentTool.ProjectHeadDirectory, 
            PathConstants.SecretsFolder, 
            $"{keyName}.json");
    }
}