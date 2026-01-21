using VSystem.Internal.Dependencies;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.Services.Registration;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Internal.ServerApiExecutors.Register;

public class UnregisterUserExecutor : ServerApiExecutor
{
    public override void Dispose()
    {
        
    }

    public override Task<ResponseDTO> Execute(RequestDTO request)
    {
        AppDependencies.Provider.Get(out IRegistrationService registrationService);

        return Task.FromResult(ResponseModelsCollection.NotImplementedResponse);
    }
}