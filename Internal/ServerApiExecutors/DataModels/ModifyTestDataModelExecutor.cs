using System.Net;
using Newtonsoft.Json;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.DataModels.Testable;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.ResponseModels.DataModels;
using VSystem.Internal.Services.Data;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.DataModels;

public class ModifyTestDataModelExecutor : ServerApiExecutor
{
    public override void Dispose() { }
   
    public override Task<ResponseDTO> Execute(RequestDTO request)
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
            
            return Task.FromResult(new ResponseDTO()
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonConvert.SerializeObject(responseData, Formatting.Indented),
            });
        }
        catch (Exception e)
        {
            loggingService.LogError("Сдохло сохранение...");

            return Task.FromResult(ResponseModelsCollection.ErrorResponse);
        }
    }
}