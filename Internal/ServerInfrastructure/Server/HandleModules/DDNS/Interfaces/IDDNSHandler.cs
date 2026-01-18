namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS;

public interface IDDNSHandler : IDisposable
{
    public string GetExternalIp();
    public Task UpdateDDNSAsync();
    public Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken);
}