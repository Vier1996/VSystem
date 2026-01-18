using VSystem.Internal.ServerInfrastructure.Server.HandleModules.DDNS;

namespace VSystem.Internal.ServerInfrastructure.Server.Configuration;

[Serializable]
public record ServerConfiguration
{
    public ServerNetworkSettings Server { get; init; }
    public DDNSSettings DDNS { get; init; }
}