using VSystem.Internal.Constants;

namespace VSystem.Internal.Environment;

public static class EnvironmentUtils
{
    public static string ProjectHeadDirectory { get; }

    static EnvironmentUtils()
    {
        string baseDirectory = System.IO.Directory.GetCurrentDirectory();
        
        ProjectHeadDirectory = baseDirectory.Replace(AppConstants.Path.ProjectRedundantPathPostfix, string.Empty); 
    }
}