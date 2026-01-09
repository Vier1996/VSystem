namespace VSystem.Internal.Dependencies;

public interface IDependenciesProvider
{
    public T Get<T>() where T : class;
    public IDependenciesProvider Get<T>(out T service) where T : class;
    public bool TryGet<T>(out T service) where T : class;
}