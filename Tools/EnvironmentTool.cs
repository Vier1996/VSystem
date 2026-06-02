using VSystem.Constants;

namespace VSystem.Tools;

public static class EnvironmentTool
{
    public static string ProjectHeadDirectory { get; private set; }
    public static string ServerDataRootDirectory { get; private set; }

    static EnvironmentTool()
    {
        SetupHeadEnvironmentDirectory();
    }

    private static void SetupHeadEnvironmentDirectory()
    {
        ProjectHeadDirectory = System.IO.Directory
            .GetCurrentDirectory()
            .Replace(PathConstants.ProjectRedundantPathPostfix, string.Empty);
    }
}