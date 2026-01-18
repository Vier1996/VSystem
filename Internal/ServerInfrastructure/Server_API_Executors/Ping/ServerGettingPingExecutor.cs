using System.Net;
using Newtonsoft.Json;
using VSystem.Internal.ServerInfrastructure.Server_API.Ping;
using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.Internal.ServerInfrastructure.Server_API_Executors.Ping;

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