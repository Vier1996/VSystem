namespace VSystem.Server.SystemServer.HandleModules.DDNS.Interfaces;

public interface IDDNSHandler : IDisposable
{
    public string GetExternalIp();
    public Task UpdateDDNSAsync();
    public Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken);
}