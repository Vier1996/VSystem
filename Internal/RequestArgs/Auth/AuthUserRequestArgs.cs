using Newtonsoft.Json;

namespace VSystem.Internal.RequestArgs.Auth;

[Serializable]
public record AuthUserRequestArgs : RequestArgsBase
{
    [JsonProperty] public string Login { get; init; }
    [JsonProperty] public string Password { get; init; }
}