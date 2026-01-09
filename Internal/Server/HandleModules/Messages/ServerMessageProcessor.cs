namespace VSystem.Internal.Server.HandleModules.Messages;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8604 // Possible null reference argument.

public class ServerMessageProcessor
{
    private readonly IClientManager _clientManager;
    private readonly ServerNetworkSettings _serverNetworkSettings;
    private readonly ApiExecutorsContainer _apiExecutorsContainer;
    private readonly ILoggingMiddleware _loggingMiddleware;

    public MessageProcessor(
        IClientManager clientManager,
        ServerNetworkSettings serverNetworkSettings,
        ApiExecutorsContainer apiExecutorsContainer,
        ILoggingMiddleware loggingMiddleware)
    {
        _clientManager = clientManager;
        _serverNetworkSettings = serverNetworkSettings;
        _apiExecutorsContainer = apiExecutorsContainer;
        _loggingMiddleware = loggingMiddleware;
    }

    public async Task ProcessMessageAsync(Socket clientSocket, string message)
    {
        await _loggingMiddleware.LogMessageReceivedAsync(clientSocket, message);
        
        RequestBase clientCommandRequest;

        try
        {
            clientCommandRequest = JsonConvert.DeserializeObject<RequestBase>(message);
        }
        catch (Exception e)
        {
            clientCommandRequest = null;
        }
        
        ValidationResult validationResult = CommandValidator.CommandValidator.ValidateCommand(clientCommandRequest);
        
        if (validationResult.IsValid == false)
        {
            await HandleClientFallback(clientSocket, new BadGateway(
                errorMessage: $"[{HttpStatusCode.BadGateway}] Invalid command: {validationResult.ErrorMessage}")
            );
            return;
        }

        try
        {
            string api = clientCommandRequest?.RequestApi ?? string.Empty;
            
            if (_apiExecutorsContainer.TryGetExecutor(api, out CommandExecutor targetExecutor) == false)
            {
                await HandleClientFallback(clientSocket, new BadGateway(
                    errorMessage: $"[{HttpStatusCode.BadGateway}] Not found Executor for API: [{api}]")
                );
                return;
            }
            
            string responseJson = await targetExecutor.Execute(new ExecutorPayload()
            {
                CommandArgs = clientCommandRequest?.RequestArgs,
                FeedbackCallback = (feedback) => _ = HandleClientFeedback(clientSocket, feedback),
            });
            
            await HandleClientFeedback(clientSocket, responseJson);
        }
        catch (Exception ex)
        {
            await HandleClientFallback(clientSocket, new InternalServerError(
                errorMessage: $"[{HttpStatusCode.InternalServerError}] Error processing message '{message}': {ex.Message}")
            );
        }
    }
    
    private async Task HandleClientFeedback(Socket clientSocket, string responseJson)
    {
        await _clientManager.SendMessageToClientAsync(clientSocket, responseJson);
    }

    private async Task HandleClientFeedback(Socket clientSocket, ResponseBase responseBase)
    {
        await _clientManager.SendMessageToClientAsync(clientSocket, responseBase);
    }
    
    private async Task HandleClientFallback(Socket clientSocket, ResponseBase responseBaseMessage)
    {
        await _clientManager.SendMessageToClientAsync(clientSocket, responseBaseMessage);
        await _loggingMiddleware.LogErrorAsync(clientSocket, responseBaseMessage.ErrorMessage);
    }
}