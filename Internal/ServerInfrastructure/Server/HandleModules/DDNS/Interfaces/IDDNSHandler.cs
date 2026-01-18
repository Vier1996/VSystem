namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS.Interfaces;

public interface IDDNSHandler : IDisposable
{
    public string GetExternalIp();
    public Task UpdateDDNSAsync();
    public Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken);
}