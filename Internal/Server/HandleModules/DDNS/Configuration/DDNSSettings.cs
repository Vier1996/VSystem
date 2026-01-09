namespace VSystem.Internal.Server.HandleModules.DDNS;

public record DDNSSettings
{
    public required string Hostname { get; init; }
    public required string ContentAddress { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public int UpdateIntervalMinutes { get; init; } = 5;
    
    //string url = $"https://dynupdate.no-ip.com/nic/update?hostname={_ddnsSettings.Hostname}&myip={externalIp}";

}