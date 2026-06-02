using VSystem.Constants;

namespace VSystem.Tools;

public static class AssemblyTool
{
    private static readonly System.Reflection.Assembly? _projectAssembly = null;
    private static readonly Type[] _assemblyTypes = [];
    
    static AssemblyTool()
    {
        _projectAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .First(assembly => assembly.GetName().Name!.Equals(AssemblyConstants.ServerAssemblyName));
        
        if (_projectAssembly != null)
            _assemblyTypes = _projectAssembly.GetTypes();
    }
    
    public static List<Type> GetTypes<TAttribute, TType>()
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