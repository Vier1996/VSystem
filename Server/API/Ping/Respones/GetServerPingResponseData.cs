namespace VSystem.Server.API.Ping.Respones;

[System.Serializable]
public record GetServerPingResponseData
{
    public int PingValue { get; init; }
};