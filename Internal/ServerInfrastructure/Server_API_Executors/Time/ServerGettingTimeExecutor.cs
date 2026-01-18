using System.Net;
using Newtonsoft.Json;
using VSystem.Internal.ServerInfrastructure.Server_API.ServerTime;
using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.Internal.ServerInfrastructure.Server_API_Executors.Time;

public class ServerGettingTimeExecutor : ServerApiExecutor
{
    public override void Dispose() { }

    public override Task<string> Execute(RequestDTO request)
    {
        DateTime serverTime = DateTime.Now;
        GetServerTimeResponseData responseData = new GetServerTimeResponseData()
        {
            Hour = serverTime.Hour,
            Minutes = serverTime.Minute,
            Seconds = serverTime.Second,
        };
        
        ResponseDTO responseDto = new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Message = JsonConvert.SerializeObject(responseData, Formatting.Indented),
        };
        
        return Task.FromResult(JsonConvert.SerializeObject(responseDto, Formatting.Indented));
    }
}