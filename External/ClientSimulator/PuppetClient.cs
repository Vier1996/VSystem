using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Server_API.Ping.Responses;
using VSystem.Internal.Server.Configuration;
using VSystem.Internal.Server.ServerDataBases;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VSystem.External.ClientSimulator;

public class PuppetClient : IDisposable
{
    private readonly ILoggingService _loggingService;
    private readonly ServerNetworkSettings _serverNetworkSettings;

    private readonly RequestDTO _testablePingRequest;
    
    public PuppetClient()
    {
        _testablePingRequest = new RequestDTO()
        {
            RequestApi = AppConstants.ServerAPI.ServerPing,
            RequestArgs = string.Empty,
        };
        
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
            "localhost",
            "94.77.147.247",
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
                
                await client.ConnectAsync(host, _serverNetworkSettings.Port);
                
                _loggingService.LogMessage(
                    message: $"✅ {host}:{_serverNetworkSettings.Port} - Подключение успешно!",
                    sender: this);

                string response = await SendPingRequest(client);

                _loggingService.LogMessage(
                    message: $"✅ ответ получен [{response}]",
                    sender: this);
                
                client.Close();
                
                await Task.Delay(TimeSpan.FromSeconds(1));
            }
            catch (Exception ex)
            {
                _loggingService.LogError(
                    message: $"❌ {host}:{_serverNetworkSettings.Port} - Ошибка: {ex.Message}",
                    sender: this);
            }
        }
    }
    
    private async Task<string> SendPingRequest(TcpClient client)
    {
        try
        {
            string json = JsonSerializer.Serialize(_testablePingRequest);
            byte[] data = Encoding.UTF8.GetBytes(json);

            NetworkStream stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length);

            byte[] buffer = new byte[2048];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string responseData = Encoding.UTF8.GetString(buffer);
            
            ResponseDTO responseDto = JsonConvert.DeserializeObject<ResponseDTO>(responseData, settings: new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            })!;
            
            GetServerPingResponseData responsePingData = JsonConvert.DeserializeObject<GetServerPingResponseData>(responseDto.Message, settings: new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            })!;
            
            return responsePingData.PingValue.ToString();
        }
        catch (Exception ex)
        {
            _loggingService.LogError(
                message: $"❌ Ошибка при отправке запроса: {ex.Message}",
                sender: this);
        }
        
        return string.Empty;
    }
}