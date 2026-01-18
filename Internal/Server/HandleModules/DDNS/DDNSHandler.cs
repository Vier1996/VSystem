using System.Net.Http.Headers;
using System.Text;
using VSystem.Internal.Constants;
using VSystem.Internal.Dependencies;
using VSystem.Internal.Logging;
using VSystem.Internal.Network;
using static System.GC;

namespace VSystem.Internal.Server.HandleModules.DDNS;

public class DDNSHandler : IDDNSHandler
{
    private readonly DDNSSettings _ddnsSettings;
    private readonly HttpClient _httpClient;
    private readonly ILoggingService _loggingService;
    
    public DDNSHandler()
    {
        _httpClient = new HttpClient();

        AppDependencies.Provider
            .Get(out _loggingService)
            .Get(out _ddnsSettings);
    }
    
    public void Dispose()
    {
        _httpClient?.Dispose();
        SuppressFinalize(this);
    }

    public string GetExternalIp()
    {
        string ip = string.Empty;
        
        try
        {
#if WINDOWS
            ip = _ddnsSettings.ContentAddress;
#elif MACOS
            ip = NetworkUtility.GetLocalIpAddress();
#endif
        }
        catch (Exception ex)
        {
            _loggingService.LogError(message: string.Format(
                format: AppConstants.DDNS.ExternalIpGettingErrorMessage,
                arg0: ex.Message));

            return string.Empty;
        }

        return ip;
    }

    public async Task UpdateDDNSAsync()
    {
        try
        {
            string externalIp = GetExternalIp();
            
            if (string.IsNullOrEmpty(externalIp))
                return;
            
            string url = string.Format(format: _ddnsSettings.Hostname, arg0: externalIp);
            byte[] bytes = Encoding.UTF8.GetBytes($"{_ddnsSettings.Username}:{_ddnsSettings.Password}");
            string authValue = Convert.ToBase64String(bytes);
          
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
            
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
         
            _loggingService.LogMessage(
                message: string.Format(AppConstants.DDNS.NewDDNSMessage, result), 
                sender: this);
        }
        catch (Exception ex)
        {
            _loggingService.LogMessage(
                message: string.Format(AppConstants.DDNS.FailedUpdateDDNSErrorMessage, ex.Message), 
                sender: this);
        }
    }

    public async Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await UpdateDDNSAsync();
            await Task.Delay(TimeSpan.FromMinutes(_ddnsSettings.UpdateIntervalMinutes), cancellationToken);
        }
    }
}