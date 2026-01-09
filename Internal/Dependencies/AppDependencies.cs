namespace VSystem.Internal.Dependencies;

public static class AppDependencies
{
    public static IDependenciesRegistrator Registrator { get; private set; }
    public static IDependenciesProvider Provider { get; private set; }

    private static readonly DependenciesManager _dependenciesManager;

    static AppDependencies()
    {
        _dependenciesManager = new DependenciesManager();
        Registrator = _dependenciesManager;
        Provider = _dependenciesManager;
    }

    public static void Dispose()
    {
        _dependenciesManager?.Dispose();
    }
}