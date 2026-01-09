namespace VSystem.Internal.Server.HandleModules.DDNS;

public interface IDDNSHandler : IDisposable
{
    public Task<string> GetExternalIpAsync();
    public Task UpdateDDNSAsync();
    public Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken);
}