namespace VSystem.Internal.ResponseModels.Register;

[System.Serializable]
public record RegisterUserResponse : ResponseModelBase
{
    public string CallbackMessage { get; init; }
}