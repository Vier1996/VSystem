namespace VSystem.Server.API.DataModel.Responses;

[System.Serializable]
public record ModifyServerTestModelResponseData
{
    public int NewValue { get; init; }
}