using System.Net;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ResponseModels._Base;

public static class ResponseModelsCollection
{
    public static ResponseDTO ErrorResponse { get; }
    public static ResponseDTO UnauthorizedResponse { get; }
    public static ResponseDTO BadRequestResponse { get; }
    public static ResponseDTO NotImplementedResponse { get; }

    static ResponseModelsCollection()
    {
        ErrorResponse = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.InternalServerError,
            Content = string.Empty,
        };
        
        UnauthorizedResponse = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.Unauthorized,
            Content = string.Empty,
        };
        
        BadRequestResponse = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.BadRequest,
            Content = string.Empty,
        };
        
        NotImplementedResponse = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.NotImplemented,
            Content = string.Empty,
        };
    }
}