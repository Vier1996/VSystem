using System.Net;
using Newtonsoft.Json;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Operations;
using VSystem.Internal.RequestArgs.Auth;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.ResponseModels.Auth;
using VSystem.Internal.ResponseModels.Register;
using VSystem.Internal.Services.Authentication;
using VSystem.Internal.Services.Data;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.Auth;

public class AuthUserExecutor : ServerApiExecutor
{
    public override void Dispose()
    {
        
    }

    public override Task<ResponseDTO> Execute(RequestDTO request)
    {
        AuthUserRequestArgs authArgs = JsonConvert.DeserializeObject<AuthUserRequestArgs>(request.RequestArgs);
        
        if (authArgs == null)
        {
            return Task.FromResult(ResponseModelsCollection.BadRequestResponse);
        }
        
        AppDependencies.Provider.Get(out IDataService dataService);
        AppDependencies.Provider.Get(out IAuthenticationService authenticationService);
        
        AuthenticationOperationCallback callback = authenticationService.AuthenticateUser(authArgs.Login, authArgs.Password);
        
        if (callback.IsSuccess == false)
        {
            return Task.FromResult(new ResponseDTO()
            {
                StatusCode =  HttpStatusCode.BadGateway,
                Content = new AuthUserResponse()
                {
                    SecureToken = callback.CallbackMessage
                }.ToJson()
            });
        }
        
        dataService.SaveAllForce();

        return Task.FromResult(new ResponseDTO()
        {
            StatusCode = HttpStatusCode.OK,
            Content = new AuthUserResponse()
            {
                SecureToken = callback.SecureToken
            }.ToJson()
        });
    }
}