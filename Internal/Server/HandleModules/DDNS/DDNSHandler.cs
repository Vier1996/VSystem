namespace VSystem.Internal.Server.HandleModules.DDNS;

public class DDNSHandler
{
    private readonly DDNSSettings _ddnsSettings;
    private readonly HttpClient _httpClient;
    private bool _disposed = false;

    public DDNSService(DDNSSettings ddnsSettings)
    {
        _ddnsSettings = ddnsSettings;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetExternalIpAsync()
    {
        return _ddnsSettings.ContentAddress;
        
        try
        {
            return await _httpClient.GetStringAsync("https://api.ipify.org");
        }
        catch (Exception ex)
        {
            this.LogError($"Failed to get external IP: {ex.Message}");
            return string.Empty;
        }
    }

    public async Task UpdateDDNSAsync()
    {
        try
        {
            string externalIp = await GetExternalIpAsync();
            if (string.IsNullOrEmpty(externalIp))
                return;

            string url = $"https://dynupdate.no-ip.com/nic/update?hostname={_ddnsSettings.Hostname}&myip={externalIp}";
            
            var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_ddnsSettings.Username}:{_ddnsSettings.Password}"));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
            
            var response = await _httpClient.GetAsync(url);
            string result = await response.Content.ReadAsStringAsync();
            
            this.LogMessage($"New DDNS: {result}");
        }
        catch (Exception ex)
        {
            this.LogError($"Failed to update DDNS: {ex.Message}");
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _httpClient?.Dispose();
            _disposed = true;
        }
    }
}