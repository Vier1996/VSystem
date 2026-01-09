using VSystem.Internal.Server.ServerDataBases;

namespace VSystem.Internal.Server_API.ServerTime.Responses;

[System.Serializable]
public record GetServerTimeResponse : ResponseBase
{
    public int Hour { get; init; }
    public int Minutes { get; init; }
    public int Seconds { get; init; }
}