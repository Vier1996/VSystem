namespace VSystem.Internal.ResponseModels.Ping;

[System.Serializable]
public record GetServerPingResponseData : ResponseModelBase
{
    public int PingValue { get; init; }
};