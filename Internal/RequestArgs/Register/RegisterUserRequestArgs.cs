using Newtonsoft.Json;

namespace VSystem.Internal.RequestArgs.Register;

[Serializable]
public record RegisterUserRequestArgs : RequestArgsBase
{
    [JsonProperty] public string Login { get; init; }
    [JsonProperty] public string Password { get; init; }
}