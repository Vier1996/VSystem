using System.Net;
using Newtonsoft.Json;

namespace VSystem.Server.SystemServer.ServerDataBases;

[System.Serializable]
public record ResponseDTO
{
    [JsonProperty] public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.Processing;
    [JsonProperty] public string Content { get; init; } = string.Empty; 
}