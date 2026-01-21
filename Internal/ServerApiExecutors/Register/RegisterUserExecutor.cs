using System.Net;
using Newtonsoft.Json;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Operations;
using VSystem.Internal.RequestArgs.Register;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.ResponseModels.Register;
using VSystem.Internal.Services.Data;
using VSystem.Internal.Services.Registration;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.Register;

public class RegisterUserExecutor : ServerApiExecutor
{
    public override void Dispose()
    {
        
    }

    public override Task<ResponseDTO> Execute(RequestDTO request)
    {
        RegisterUserRequestArgs registerArgs = JsonConvert.DeserializeObject<RegisterUserRequestArgs>(request.RequestArgs);
        
        if (registerArgs == null)
        {
            return Task.FromResult(ResponseModelsCollection.BadRequestResponse);
        }
        
        AppDependencies.Provider.Get(out IDataService dataService);
        AppDependencies.Provider.Get(out IRegistrationService registrationService);
        
        ServerOperationCallback callback = registrationService.RegisterUser(registerArgs.Login, registerArgs.Password);
        
        if (callback.IsSuccess)
            dataService.SaveAllForce();
        
        return Task.FromResult(new ResponseDTO()
        {
            StatusCode = callback.IsSuccess 
                ? HttpStatusCode.OK
                : HttpStatusCode.BadGateway,
            Content = new RegisterUserResponse()
            {
                CallbackMessage = callback.CallbackMessage
            }.ToJson()
        });
    }
}