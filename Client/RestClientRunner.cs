using VSystem.Internal.Keys;
using VSystem.Internal.Logging;
using VSystem.Tools;

namespace VSystem.Client;

public class RestClientRunner
{
    private readonly RunClientKey _runKey;
    private readonly ILoggingService _loggingService;

    public RestClientRunner()
    {
        _runKey = KeyGetTool.GetKey<RunClientKey>();
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        using RestApiClient apiClient = new RestApiClient(_runKey);

        _loggingService.LogMessage($"Connecting to server at {_runKey.ServerUrl}", this);

        try
        {
            string pingResponse = await apiClient.GetPingAsync(cancellationToken);
            _loggingService.LogMessage($"GET /api/server/ping -> {pingResponse}", this);

            string timeResponse = await apiClient.GetTimeAsync(cancellationToken);
            _loggingService.LogMessage($"GET /api/server/time -> {timeResponse}", this);
        }
        catch (Exception ex)
        {
            _loggingService.LogError($"Client request failed: {ex.Message}", this);
            throw;
        }
    }
}
