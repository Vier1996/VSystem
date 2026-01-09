using Newtonsoft.Json;

namespace VSystem.Internal.Server.ServerDataBases;

[System.Serializable]
public abstract record RequestBase
{
    [JsonProperty] public string RequestApi { get; init; } = string.Empty;
    [JsonProperty] public string RequestArgs { get; init; } = string.Empty;
}