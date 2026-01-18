namespace VSystem.Internal.ServerInfrastructure.ServerAPI.DataModel;

[System.Serializable]
public record ModifyServerTestModelResponseData
{
    public int NewValue { get; init; }
}