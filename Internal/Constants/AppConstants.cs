namespace VSystem.Internal.Constants;

public class AppConstants
{
    public static class Assembly
    {
        public const string ServerAssemblyName = "VSystem";
    }
    
    public static class Logging
    {
        public const string DefaultSenderName = "Internal";
    }
    
    public static class Path
    {
        public const string ProjectPathPostfix = "bin/Debug/net9.0";
        public const string ServerResourcesPath = @"ServerResources";
    }
    
    public static class Credential
    {
        public static readonly string FileName = "Credentials.json";
        public static readonly string ServerDataName = "Server";
        public const string DynamicDnsName = "DDNS";
    }
    
    public static class Data
    {
        public const string ServerModelLocalPath = "Data/ServerModels";
        public const string UserModelLocalPath = "Data/UserModels";
        public const string Extension = ".visd";
    }

    public static class Server
    {
        public const string ClientConnectedMessage = "Client with endpoint {0} connected!";
        public const string ClientDisconnectedMessage = "Client with endpoint {0} disconnected!";
        public const string ClientHandlingError = "Client with endpoint {0} fauted with error - {1}";
    }

    public static class Dependencies
    {
        public const string AlreadyRegisterDependencyWarningMessage = "Service of type [{0}] already registered!";
        public const string NotFoundDependencyErrorMessage = "Service of type [{0}] not found at container!";
    }
}