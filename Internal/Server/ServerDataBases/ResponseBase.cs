using System.Net;

namespace VSystem.Internal.Server.ServerDataBases;

[System.Serializable]
public abstract record ResponseBase
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.Processing;
    public string ErrorMessage { get; init; } = string.Empty;
}