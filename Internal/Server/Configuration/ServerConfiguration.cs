using VSystem.Internal.Server.HandleModules.DDNS;

namespace VSystem.Internal.Server.Configuration;

public record ServerConfiguration
{
    public ServerNetworkSettings Server { get; init; }
    public DDNSSettings DDNS { get; init; }
}