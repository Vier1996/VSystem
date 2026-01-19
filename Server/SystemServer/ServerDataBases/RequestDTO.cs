using Newtonsoft.Json;

namespace VSystem.Server.SystemServer.ServerDataBases;

[System.Serializable]
public record RequestDTO
{
    [JsonProperty] public string RequestApi { get; init; } = string.Empty;
    [JsonProperty] public string RequestArgs { get; init; } = string.Empty;
}