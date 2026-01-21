namespace VSystem.Internal.ResponseModels.Auth;

[Serializable]
public record AuthUserResponse : ResponseModelBase
{
    public string SecureToken { get; init; }
}