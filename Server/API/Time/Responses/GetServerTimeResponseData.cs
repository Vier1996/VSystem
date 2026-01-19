namespace VSystem.Server.API.Time.Responses;

[System.Serializable]
public record GetServerTimeResponseData
{
    public int Hour { get; init; }
    public int Minutes { get; init; }
    public int Seconds { get; init; }
}