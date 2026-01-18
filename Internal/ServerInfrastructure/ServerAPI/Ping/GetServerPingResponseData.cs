namespace VSystem.Internal.ServerInfrastructure.ServerAPI.Ping;

[System.Serializable]
public record GetServerPingResponseData
{
    public int PingValue { get; init; }
};