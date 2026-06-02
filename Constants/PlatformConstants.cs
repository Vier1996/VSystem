namespace VSystem.Constants;

public static class PlatformConstants
{
    public const string Platform = 
#if WINDOWS
        "Windows";
#elif MACOS
        "macOS";
#endif
        
    public const string Separator = 
#if WINDOWS
        @"\";
#elif MACOS
        "/";
#endif
}