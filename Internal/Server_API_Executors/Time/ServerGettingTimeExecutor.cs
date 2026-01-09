using System.Net;
using System.Text.Json;
using VSystem.Internal.Server_API.ServerTime.Requests;
using VSystem.Internal.Server_API.ServerTime.Responses;
using VSystem.Internal.Server.ServerDataBases;

namespace VSystem.Internal.Server_API_Executors.Time;

public class ServerGettingTimeExecutor : ServerApiExecutor
{
    public override void Dispose() { }

    public override Task<string> Execute(RequestBase request)
    {
        if (request is not GetServerTimeRequest getServerTimeRequest) 
            return Task.FromResult(string.Empty);
        
        DateTime now = DateTime.Now;
        GetServerTimeResponse responseData = new GetServerTimeResponse()
        {
            Hour = now.Hour,
            Minutes = now.Minute,
            Seconds = now.Second,

            StatusCode = HttpStatusCode.OK,
            ErrorMessage = string.Empty,
        };
        
        return Task.FromResult(JsonSerializer.Serialize(responseData));
    }
}