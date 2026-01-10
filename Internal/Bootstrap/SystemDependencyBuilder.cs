using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Environment;
using VSystem.Internal.Logging;
using VSystem.Internal.Server;
using VSystem.Internal.Server.Configuration;
using VSystem.Internal.Server.HandleModules.Clients;
using VSystem.Internal.Server.HandleModules.Clients.Interfaces;
using VSystem.Internal.Server.HandleModules.DDNS;
using VSystem.Internal.Server.HandleModules.Messages;
using VSystem.Internal.Server.HandleModules.Messages.Interfaces;

namespace VSystem.Internal.Bootstrap;

public class SystemDependencyBuilder : IDisposable
{
    public void Dispose()
    {
        AppDependencies.Dispose();
    }

    public Task ResolveDependencies()
    {
        ServerConfiguration serverConfiguration = GetServerConfiguration();
        AppDependencies.Registrator
            .Register(serverConfiguration.Server)
            .Register(serverConfiguration.DDNS)
            .Register(typeof(ILoggingService), new LoggingService())
            .Register(new ServerApiExecutorsBridge())
            .Register(typeof(IClientsHandler), new ClientsHandler())
            .Register(typeof(IDDNSHandler), new DDNSHandler())
            .Register(typeof(IServerMessageProcessor), new ServerMessageProcessor())
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