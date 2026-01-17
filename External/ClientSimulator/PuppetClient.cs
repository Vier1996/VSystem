using System.Net.Sockets;
using VSystem.External.ClientSimulator.ClientLogic;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.ServerInfrastructure.Server.Configuration;

namespace VSystem.External.ClientSimulator;

public class PuppetClient : IDisposable
{
    private readonly ILoggingService _loggingService;
    private readonly ServerNetworkSettings _serverNetworkSettings;
    
    public PuppetClient()
    {
        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out _serverNetworkSettings)
            ;
    }
    
    public void Dispose()
    {
        
    }

    public async Task RunClient()
    {
        List<string> hosts = new()
        {
            //"127.0.0.1",
            "vserversystem.ddns.net",
        };
        
        foreach (string host in hosts)
        {
            _loggingService.LogMessage(
                message: $"Тестирование {host}:{_serverNetworkSettings.Port}",
                sender: this);
            
            try
            {
                using var client = new TcpClient();
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                
                await client.ConnectAsync(host, _serverNetworkSettings.Port/*, cts.Token*/);
                
                _loggingService.LogMessage(message: $"✅ {host}:{_serverNetworkSettings.Port} - Подключение успешно!", sender: this);
                
                string response = await RunRequestLogic(client);
                
                _loggingService.LogMessage(message: $"✅ Ответ: [{response}]", sender: this);
                
                client.Close();
                
                await Task.Delay(TimeSpan.FromSeconds(1), cts.Token);
            }
            catch (SocketException se)
            {
                _loggingService.LogError(
                    message: $"❌ Socket ошибка {host}:{_serverNetworkSettings.Port} - Код: {se.SocketErrorCode}, Сообщение: {se.Message}",
                    sender: this);
            }
            catch (Exception ex)
            {
                _loggingService.LogError(
                    message: $"❌ {host}:{_serverNetworkSettings.Port} - Ошибка: {ex.Message}",
                    sender: this);
            }
        }
    }

    public async Task<string> RunRequestLogic(TcpClient client)
    {
        IClientLogicVariant logic = new ClientGetPingLogic();
        string response;

        try
        {
            response = await logic.Run(client);
        }
        catch (Exception ex)
        {
            response = string.Empty;
                    
            _loggingService.LogError(
                message: $"❌ Ошибка при отправке запроса: {ex.Message}",
                sender: this);
        }
        
        return response;
    }
}