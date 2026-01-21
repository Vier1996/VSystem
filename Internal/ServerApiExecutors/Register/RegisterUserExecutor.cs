using System.Net;
using Newtonsoft.Json;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.Dependencies;
using VSystem.Internal.RequestArgs.Register;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.ResponseModels.Register;
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
        
        AppDependencies.Provider.Get(out IRegistrationService registrationService);
        
        if (registrationService.IsRegisteredUser(registerArgs.Login))
            return Task.FromResult(new ResponseDTO()
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = "User already exists!"
            });


        RegisterUserResponse response = new RegisterUserResponse()
        {
            Message = "User success register!"
        };
        
        return Task.FromResult(new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Content = response.ToJson()
        });
    }
}