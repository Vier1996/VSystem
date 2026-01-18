namespace VSystem.Internal.ServerInfrastructure.Server;

[Serializable]
public record ServerNetworkSettings
{
    public int Port { get; init; }
    public float ResponseDelaySeconds { get; init; }
    public int MaxConnections { get; init; }
    public int BufferSize { get; init; }
}