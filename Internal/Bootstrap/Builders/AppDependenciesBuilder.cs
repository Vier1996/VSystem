using VSystem.Internal.Assembly;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Operations;
using VSystem.Internal.Services.Authentication;
using VSystem.Internal.Services.Data;
using VSystem.Internal.Services.Registration;
using VSystem.Internal.Services.Token;

namespace VSystem.Internal.Bootstrap;

public class AppDependenciesBuilder : IDependencyBuilder
{
    public void Dispose()
    {
    }

    public Task<ServerOperationCallback> Run(CancellationTokenSource appCancellationToken)
    {
        try
        {
            AppDependencies.Registrator
                .Register(new AssemblyManager())
                .Register(typeof(ILoggingService), new LoggingService())
                .Register(typeof(IDataService), new DataService(new DataServiceInitializeArgs()
                {
                    AutoSaveDelay = AppConstants.Data.AutoSaveModelsDataDelay,
                    PathInitializeArgs = new DataPathManager.DataPathInitializeArgs()
                    {
                        ServerModelDirectoryPath = AppConstants.Data.RootServerDataModelsPath,
                        UserModelDirectoryPath = AppConstants.Data.RootUserDataModelsPath,
                        DataExtension = AppConstants.Data.ModelsDataExtension,
                    }
                }))
                .Register(typeof(IRegistrationService), new RegistrationService())
                .Register(typeof(ISecureTokenService), new SecureTokenService())
                .Register(typeof(IAuthenticationService), new AuthenticationService())
                ;

            return Task.FromResult(new ServerOperationCallback()
            {
                IsSuccess = true,
                CallbackMessage = "OK"
            });
        }
        catch (Exception e)
        {
            return Task.FromResult(new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = e.Message
            });
        }
    }
}