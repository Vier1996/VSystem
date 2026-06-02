using Newtonsoft.Json;

namespace VSystem.Internal.Keys;

public record RunClientKey : IApiKey
{
    [JsonProperty] public string ServerUrl { get; init; }
}