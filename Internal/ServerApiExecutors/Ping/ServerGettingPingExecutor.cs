using System.Net;
using Newtonsoft.Json;
using VSystem.Internal.ResponseModels.Ping;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.Ping;

public class ServerGettingPingExecutor : ServerApiExecutor
{
    public override void Dispose() { }

    public override Task<ResponseDTO> Execute(RequestDTO request)
    {
        GetServerPingResponseData responseData = new GetServerPingResponseData()
        {
            PingValue = 1
        };
        
        return Task.FromResult(new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonConvert.SerializeObject(responseData, Formatting.Indented),
        });
    }
}