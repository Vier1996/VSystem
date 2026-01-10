namespace VSystem.Internal.Server_API.Ping.Responses;

[System.Serializable]
public record GetServerPingResponseData
{
    public int PingValue { get; init; }
};