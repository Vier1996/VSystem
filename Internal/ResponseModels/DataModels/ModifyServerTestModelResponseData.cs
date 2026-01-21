namespace VSystem.Internal.ResponseModels.DataModels;

[System.Serializable]
public record ModifyServerTestModelResponseData : ResponseModelBase
{
    public int NewValue { get; init; }
}