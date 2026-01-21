namespace VSystem.Internal.ResponseModels.Time;

[System.Serializable]
public record GetServerTimeResponseData : ResponseModelBase
{
    public int Hour { get; init; }
    public int Minutes { get; init; }
    public int Seconds { get; init; }
}