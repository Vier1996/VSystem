using VSystem.Server.SystemServer.HandleModules.DDNS.Configuration;

namespace VSystem.Server.SystemServer.Configuration;

[Serializable]
public record ServerConfiguration
{
    public ServerNetworkSettings Server { get; init; }
    public DDNSSettings DDNS { get; init; }
}