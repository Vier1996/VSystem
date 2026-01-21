using System.Net;

namespace VSystem.Server.SystemServer.ServerDataBases;

[System.Serializable]
public record ResponseDTO
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.Processing;
    public string Content { get; init; } = string.Empty; 
}