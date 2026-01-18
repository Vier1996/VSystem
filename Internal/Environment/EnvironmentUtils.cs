using VSystem.Internal.Constants;

namespace VSystem.Internal.Environment;

public static class EnvironmentUtils
{
    public static string ProjectHeadDirectory { get; private set; }
    public static string ServerDataRootDirectory { get; private set; }

    static EnvironmentUtils()
    {
        SetupHeadEnvironmentDirectory();
        SetupDatasEnvironmentDirectory();
    }

    private static void SetupHeadEnvironmentDirectory()
    {
        ProjectHeadDirectory = System.IO.Directory
            .GetCurrentDirectory()
            .Replace(AppConstants.Path.ProjectRedundantPathPostfix, string.Empty);
    }

    private static void SetupDatasEnvironmentDirectory()
    {
        ServerDataRootDirectory = AppConstants.Data.RootDataFolderPath;
    }
}