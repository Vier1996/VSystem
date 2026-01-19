using VSystem.Internal.Dependencies;
using VSystem.Internal.Operations;
using VSystem.Server.API;
using VSystem.Server.SystemServer;
using VSystem.Server.SystemServer.HandleModules.Clients;
using VSystem.Server.SystemServer.HandleModules.Clients.Interfaces;
using VSystem.Server.SystemServer.HandleModules.DDNS;
using VSystem.Server.SystemServer.HandleModules.DDNS.Interfaces;
using VSystem.Server.SystemServer.HandleModules.Messages;
using VSystem.Server.SystemServer.HandleModules.Messages.Interfaces;

namespace VSystem.Internal.Bootstrap;

public class ServerRunner : IDependencyBuilder
{
    private ISystemServer _systemServer;

    public void Dispose()
    {
        _systemServer?.Dispose();
    }
    
    public async Task<ServerOperationCallback> Run(CancellationTokenSource appCancellationToken)
    {
        try
        {
            AppDependencies.Registrator
                .Register(new ServerApiExecutorsBridge())
                .Register(typeof(IClientsHandler), new ClientsHandler())
                .Register(typeof(IDDNSHandler), new DDNSHandler())
                .Register(typeof(IServerMessageProcessor), new ServerMessageProcessor())
                ;
            
            _systemServer = await new SystemServer().StartAsync();
            
            return new ServerOperationCallback()
            {
                IsSuccess = true,
                CallbackMessage = "OK"
            };
        }
        catch (Exception e)
        {
            return new ServerOperationCallback()
            {
                IsSuccess = false,
                CallbackMessage = e.Message
            };
        }
    }
}