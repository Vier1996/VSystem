namespace VSystem.Internal.ServerInfrastructure.Server_API.Ping;

[System.Serializable]
public record GetServerPingResponseData
{
    public int PingValue { get; init; }
};