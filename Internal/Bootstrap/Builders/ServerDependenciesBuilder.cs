using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Environment;
using VSystem.Internal.Operations;
using VSystem.Server.SystemServer.Configuration;

namespace VSystem.Internal.Bootstrap;

public class ServerDependenciesBuilder : IDependencyBuilder
{
    public void Dispose() { }
    
    public Task<ServerOperationCallback> Run(CancellationTokenSource appCancellationToken)
    {
        try
        {
            ServerConfiguration serverConfiguration = GetServerConfiguration();

            AppDependencies.Registrator
                .Register(serverConfiguration.Server)
                .Register(serverConfiguration.DDNS);
            
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