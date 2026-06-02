using Newtonsoft.Json;

namespace VSystem.Internal.Keys;

[Serializable]
public record RunServerKey : IApiKey
{
    [JsonProperty] public string Host { get; init; }
    [JsonProperty] public int Port { get; init; }
}