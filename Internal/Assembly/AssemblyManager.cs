using VSystem.Internal.Constants;

namespace VSystem.Internal.Assembly;

public class AssemblyManager
{
    private readonly System.Reflection.Assembly? _projectAssembly = null;
    private readonly Type[] _assemblyTypes = [];
    
    public AssemblyManager()
    {
        _projectAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .First(assembly => assembly.GetName().Name!.Equals(AppConstants.Assembly.ServerAssemblyName));
        
        if (_projectAssembly != null)
            _assemblyTypes = _projectAssembly.GetTypes();
    }
    
    public List<Type> GetTypes<TAttribute, TType>()
    {
        List<Type> types = new List<Type>();

        if (_projectAssembly == null)
            return types;
        
        foreach(Type type in _assemblyTypes)
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).Length > 0 &&
                !type.Name.Equals(typeof(TType).Name))
                types.Add(type);
        }
 
        return types;
    }
}