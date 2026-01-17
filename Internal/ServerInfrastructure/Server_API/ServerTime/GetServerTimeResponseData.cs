namespace VSystem.Internal.ServerInfrastructure.Server_API.ServerTime;

[System.Serializable]
public record GetServerTimeResponseData
{
    public int Hour { get; init; }
    public int Minutes { get; init; }
    public int Seconds { get; init; }
}