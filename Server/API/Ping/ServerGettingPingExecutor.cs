using System.Net;
using Newtonsoft.Json;
using VSystem.Server.API.Ping.Respones;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.API.Ping;

public class ServerGettingPingExecutor : ServerApiExecutor
{
    public override void Dispose() { }

    public override Task<string> Execute(RequestDTO request)
    {
        GetServerPingResponseData responseData = new GetServerPingResponseData()
        {
            PingValue = 1
        };
        
        ResponseDTO responseDto = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Message = JsonConvert.SerializeObject(responseData, Formatting.Indented),
        };
        
        return Task.FromResult(JsonConvert.SerializeObject(responseDto, Formatting.Indented));
    }
}