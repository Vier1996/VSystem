using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.ResponseModels._Base;
using VSystem.Internal.ServerApiExecutors;
using VSystem.Server.SystemServer.Configuration;
using VSystem.Server.SystemServer.HandleModules.Clients.Interfaces;
using VSystem.Server.SystemServer.HandleModules.Messages.Interfaces;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.Server.SystemServer.HandleModules.Messages;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

public class ServerMessageProcessor : IServerMessageProcessor
{
    private readonly IClientsHandler _clientsHandler;
    private readonly ILoggingService _loggingService;
    private readonly ServerNetworkSettings _serverNetworkSettings;
    private readonly ServerApiExecutorsBridge _serverApiExecutorsBridge;

    public ServerMessageProcessor()
    {
        AppDependencies.Provider
            .Get(out _clientsHandler)
            .Get(out _loggingService)
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

            ResponseDTO response = await executor.Execute(clientCommandRequest);

            if (response == null)
                throw new ArgumentException($"Response [{api}] can not be null!");
            
            await _clientsHandler.SendMessageToClientAsync(
                clientSocket: clientSocket,
                responseJson: response.ToJson());
        }
        catch (Exception ex)
        {
            _loggingService.LogError(
                message: ex.Message, 
                sender: this);
            
            await _clientsHandler.SendMessageToClientAsync(
                clientSocket: clientSocket,
                responseJson: ResponseModelsCollection.ErrorResponse.ToJson());
        }
        finally
        {
            executor?.Dispose();
        }
    }
}