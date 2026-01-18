using Newtonsoft.Json;
using VSystem.Internal.Assembly;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Environment;
using VSystem.Internal.Logging;
using VSystem.Internal.ServerInfrastructure.Server;
using VSystem.Internal.ServerInfrastructure.Server.Configuration;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.Clients;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.Messages;
using VSystem.Internal.Services.Data;
using VSystem.Internal.Services.Data.Interfaces;
using VSystem.Internal.Services.Data.Modules;
using VSystem.Internal.Services.Registration;

namespace VSystem.Internal.Bootstrap;

public class SystemDependencyBuilder : IDisposable
{
    public void Dispose()
    {
        AppDependencies.Dispose();
    }

    public Task ResolveDependencies()
    {
        AppDependencies.Registrator
            .Register(new AssemblyManager())
            .Register(typeof(ILoggingService), new LoggingService());
        
        ServerConfiguration serverConfiguration = GetServerConfiguration();
        
        AppDependencies.Registrator
            .Register(serverConfiguration.Server)
            .Register(serverConfiguration.DDNS)
            
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
            
            .Register(new ServerApiExecutorsBridge())
            .Register(typeof(IClientsHandler), new ClientsHandler())
            .Register(typeof(IDDNSHandler), new DDNSHandler())
            .Register(typeof(IServerMessageProcessor), new ServerMessageProcessor())
            
            .Register(typeof(IRegistrationService), new RegistrationService())
            ;
        
        return Task.CompletedTask;
    }

    private ServerConfiguration GetServerConfiguration()
    {
        string credentialPath = Path.Combine(
            path1: EnvironmentUtils.ProjectHeadDirectory, 
            path2: AppConstants.Path.ServerResourcesPath,
            path3: AppConstants.Credential.FileName);

        if (File.Exists(credentialPath) == false)
        {
            Console.WriteLine(AppConstants.Bootstrapper.NoCredentialsErrorMessage);
            return null;
        }
        
        string credentialData = File.ReadAllText(credentialPath);

        if (string.IsNullOrEmpty(credentialData))
        {
            Console.WriteLine(AppConstants.Bootstrapper.CredentialsEmptyDataErrorMessage);
            return null;
        }
        
        ServerConfiguration configuration = JsonConvert.DeserializeObject<ServerConfiguration>(credentialData)!;

        if (configuration.Server == null)
        {
            Console.WriteLine(AppConstants.Bootstrapper.NotFoundServerSettingErrorMessage);
            return null;
        }
        
        if (configuration.DDNS == null)
        {
            Console.WriteLine(AppConstants.Bootstrapper.NotFoundDDNSSettingErrorMessage);
            return null;
        }

        return configuration;
    }
}