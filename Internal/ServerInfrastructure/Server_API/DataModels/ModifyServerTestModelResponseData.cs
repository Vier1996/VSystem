namespace VSystem.Internal.ServerInfrastructure.Server_API.DataModels;

[System.Serializable]
public record ModifyServerTestModelResponseData
{
    public int NewValue { get; init; }
}