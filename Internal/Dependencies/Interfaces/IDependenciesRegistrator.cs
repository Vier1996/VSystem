namespace VSystem.Internal.Dependencies;

public interface IDependenciesRegistrator
{
    public IDependenciesRegistrator Register<T>(T service);
    public IDependenciesRegistrator Register(Type type, object service);
}