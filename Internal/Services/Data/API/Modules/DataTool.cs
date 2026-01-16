namespace V_Server.ServerExternal.Services.Data.API.Modules;

public class DataTool
{
    private readonly DataToolInitializeArgs _initializeArgs;
    private readonly System.Reflection.Assembly? _sharpAssembly = null;
    
    public DataTool(DataToolInitializeArgs initializeArgs)
    {
        _initializeArgs = initializeArgs;
        
        _sharpAssembly = AppDomain
            .CurrentDomain
            .GetAssemblies()
            .First(assembly => assembly.GetName().Name.Equals(_initializeArgs.ProjectAssembly));
    }
    
    public List<Type> GetTypes<TAttribute, TType>()
    {
        List<Type> types = new List<Type>();
 
        foreach(Type type in _sharpAssembly.GetTypes())
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).Length > 0 &&
                !type.Name.Equals(typeof(TType).Name))
                types.Add(type);
        }
 
        return types;
    }
    
    public record DataToolInitializeArgs
    {
        public required string ProjectAssembly { get; init; }
    }
}