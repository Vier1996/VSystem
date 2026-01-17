namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS.Interfaces;

public interface IDDNSHandler : IDisposable
{
    public Task<string> GetExternalIpAsync();
    public Task UpdateDDNSAsync();
    public Task StartPeriodicUpdatesAsync(CancellationToken cancellationToken);
}