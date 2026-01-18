using Newtonsoft.Json;

namespace VSystem.Internal.DataModels.Server.User;

[Serializable]
public record UserEntranceModel
{
    [JsonProperty] public Guid Guid { get; private set; } = Guid.Empty;
    [JsonProperty] public string Login { get; private set; } = string.Empty;
    [JsonProperty] public string Password { get; private set; } = string.Empty;

    public UserEntranceModel SetGuid(Guid guid)
    {
        Guid = guid;
        return this;
    }

    public UserEntranceModel SetLogin(string login)
    {
        Login = login;
        return this;
    }

    public UserEntranceModel SetPassword(string password)
    {
        Password = password;
        return this;
    }
}