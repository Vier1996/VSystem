using System.Net;
using Newtonsoft.Json;
using VSystem.Internal.ResponseModels.Time;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.Time;

public class ServerGettingTimeExecutor : ServerApiExecutor
{
    public override void Dispose() { }

    public override Task<ResponseDTO> Execute(RequestDTO requestDto)
    {
        DateTime serverTime = DateTime.Now;
        GetServerTimeResponseData responseData = new GetServerTimeResponseData()
        {
            Hour = serverTime.Hour,
            Minutes = serverTime.Minute,
            Seconds = serverTime.Second,
        };
        
        return Task.FromResult(new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonConvert.SerializeObject(responseData, Formatting.Indented),
        });
    }
}