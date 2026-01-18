namespace VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS;

[Serializable]
public record DDNSSettings
{
    public required string WebIpHost { get; init; }
    public required string Hostname { get; init; }
    public required string ContentAddress { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
    public int UpdateIntervalMinutes { get; init; } = 5;
}