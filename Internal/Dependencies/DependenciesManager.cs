using VSystem.Internal.Constants;
using VSystem.Internal.Logging;

namespace VSystem.Internal.Dependencies;

public class DependenciesManager : IDependenciesRegistrator, IDependenciesProvider, IDisposable
{
    public IEnumerable<object> RegisteredDependencies => _dependencies.Values;
    public IEnumerable<Type> RegisteredDependencyTypes => _dependencies.Keys.ToList();
    
    private readonly Dictionary<Type, object> _dependencies = new();
    
    public void Dispose()
    {
        foreach (KeyValuePair<Type, object> service in _dependencies)
            if (service.Value is IDisposable disposableService) 
                disposableService.Dispose();
    }
    
    public IDependenciesRegistrator Register<T>(T service)
    {
        Type type = typeof(T);
        
        if (!_dependencies.TryAdd(type, service))
        {
            string warningMessage = string.Format(
                format: AppConstants.Dependencies.AlreadyRegisterDependencyWarningMessage,
                arg0: type.FullName);
            
            Console.WriteLine($"[DependencyManager] (WARNING) | {warningMessage}");
        }

        return this;
    }

    public IDependenciesRegistrator Register(Type type, object service)
    {
        if (!type.IsInstanceOfType(service))
            throw new ArgumentException("Type of service does not match type of service interface",
                nameof(service));

        if (!_dependencies.TryAdd(type, service))
        {
            string warningMessage = string.Format(
                format: AppConstants.Dependencies.AlreadyRegisterDependencyWarningMessage,
                arg0: type.FullName);
            
            Console.WriteLine($"[DependencyManager] (WARNING) | {warningMessage}");
        }
        
        return this;
    }
    
    public T Get<T>() where T : class
    {
        Type type = typeof(T);

        if (_dependencies.TryGetValue(type, out object service) == false)
        {
            string warningMessage = string.Format(
                format: AppConstants.Dependencies.NotFoundDependencyErrorMessage,
                arg0: type.FullName);
            
            Console.WriteLine($"[DependencyManager] (ERROR) | {warningMessage}");
        }
            
        return service as T;
    }
    
    public IDependenciesProvider Get<T>(out T service) where T : class
    {
        Type type = typeof(T);
        service = null;
        
        if (_dependencies.TryGetValue(type, out object @object) == false)
        {
            string warningMessage = string.Format(
                format: AppConstants.Dependencies.NotFoundDependencyErrorMessage,
                arg0: type.FullName);
            
            Console.WriteLine($"[DependencyManager] (ERROR) | {warningMessage}");
            
            return this;
        }
        
        service = @object as T;
        
        return this;
    }
    
    public bool TryGet<T>(out T service) where T : class
    {
        Type type = typeof(T);
        service = null;

        if (_dependencies.TryGetValue(type, out object @object) == false)
        {
            string warningMessage = string.Format(
                format: AppConstants.Dependencies.NotFoundDependencyErrorMessage,
                arg0: type.FullName);
            
            Console.WriteLine($"[DependencyManager] (ERROR) | {warningMessage}");
            
            return false;
        }

        service = @object as T;

        return true;
    }
}