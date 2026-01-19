using System.Net;
using Newtonsoft.Json;
using VSystem.Internal.DataModels.Testable;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Services.Data;
using VSystem.Server.API.DataModel.Responses;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.API.DataModel;

public class ModifyTestDataModelExecutor : ServerApiExecutor
{
    public override void Dispose() { }
   
    public override Task<string> Execute(RequestDTO request)
    {
        AppDependencies.Provider
            .Get(out ILoggingService loggingService)
            .Get(out IDataService dataService);

        ModifyServerTestModelResponseData responseData;
        
        try
        {
            int newValue = new System.Random().Next(1, 100);
            
            dataService
                .ResolveServerData<TestServerDataModel>()
                .SetTestNumber(newValue);
            
            dataService.SaveAllForce();
            
            responseData = new ModifyServerTestModelResponseData()
            {
                NewValue = newValue,
            };
        
            ResponseDTO responseDto = new ResponseDTO()
            {
                StatusCode = HttpStatusCode.OK,
                Message = JsonConvert.SerializeObject(responseData, Formatting.Indented),
            };
        
            return Task.FromResult(JsonConvert.SerializeObject(responseDto, Formatting.Indented));
        }
        catch (Exception e)
        {
            ResponseDTO responseDto = new ResponseDTO()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = JsonConvert.SerializeObject(string.Empty, Formatting.Indented),
            };

            loggingService.LogError("Сдохло сохранение...");

            return Task.FromResult(JsonConvert.SerializeObject(responseDto, Formatting.Indented));
        }
    }
}