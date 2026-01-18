using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.ServerInfrastructure.Server_API_Executors;
using VSystem.Internal.ServerInfrastructure.Server.HandleModules.Clients;
using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.Messages;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

public class ServerMessageProcessor : IServerMessageProcessor
{
    private readonly IClientsHandler _clientsHandler;
    private readonly ServerNetworkSettings _serverNetworkSettings;
    private readonly ServerApiExecutorsBridge _serverApiExecutorsBridge;

    public ServerMessageProcessor()
    {
        AppDependencies.Provider
            .Get(out _clientsHandler)
            .Get(out _serverNetworkSettings)
            .Get(out _serverApiExecutorsBridge);
    }

    public async Task ProcessMessageAsync(Socket clientSocket, string message)
    {
        RequestDTO clientCommandRequest;
        ServerApiExecutor executor = null;
        
        try
        {
            clientCommandRequest = JsonConvert.DeserializeObject<RequestDTO>(message);
        }
        catch (Exception e)
        {
            clientCommandRequest = null;
        }

        try
        {
            string api = clientCommandRequest?.RequestApi ?? string.Empty;

            if (_serverApiExecutorsBridge.TryGetExecutor(api, out executor) == false)
            {
                await _clientsHandler.SendMessageToClientAsync(
                    clientSocket: clientSocket,
                    responseJson: string.Format(format: AppConstants.Server.NotFoundExecutorForRequestErrorMessage,
                        arg0: HttpStatusCode.BadGateway,
                        arg1: api));

                return;
            }

            string responseJson = await executor.Execute(clientCommandRequest);
            
            await _clientsHandler.SendMessageToClientAsync(
                clientSocket: clientSocket,
                responseJson: responseJson);
        }
        catch (Exception ex)
        {
            await _clientsHandler.SendMessageToClientAsync(
                clientSocket: clientSocket,
                responseJson: string.Format(format: AppConstants.Server.FailedExecutingToClientResponseMessage,
                    arg0: HttpStatusCode.InternalServerError,
                    arg1: message,
                    arg2: ex.Message));
        }
        finally
        {
            executor?.Dispose();
        }
    }
}