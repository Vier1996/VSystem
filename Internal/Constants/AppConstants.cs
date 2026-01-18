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
        public const string ProjectRedundantPathPostfix = 
#if WINDOWS
            @"bin\Debug\net9.0";
#elif MACOS
            @"bin/Debug/net9.0";
#endif
        
        public const string ServerResourcesPath = 
#if WINDOWS
            @"External\ServerResources";
#elif MACOS
            @"External/ServerResources";
#endif
    }
    
    public static class Credential
    {
        public static readonly string FileName = "Credentials.json";
        public static readonly string ServerDataName = "Server";
        public const string DynamicDnsName = "DDNS";
    }
    
    public static class Data
    {
        public const int AutoSaveModelsDataDelay = 60;
        
        public const string RootDataFolderPath =  @"C:/ServerData";
        public const string RootDataModelsPath =  @$"{RootDataFolderPath}/DataModels";
        public const string RootDataMediaPath =  @$"{RootDataFolderPath}/MediaData";
        public const string RootServerDataModelsPath =  @$"{RootDataModelsPath}/ServerDataModels";
        public const string RootUserDataModelsPath =  @$"{RootDataModelsPath}/UserDataModels";
        public const string ModelsDataExtension = ".visd";
        
        public const string FailureDataModelSavingMessage = "Saving model [{0}] at path [{1}] finished with Exception by: {2}";
    }

    public static class DDNS
    {
        public const string ExternalIpGettingErrorMessage = "Failed to get external IP: {0}";
        public const string NewDDNSMessage = "New DDNS: {0}";
        public const string FailedUpdateDDNSErrorMessage = "Failed to update DDNS: {0}";
    }

    public static class Dependencies
    {
        public const string AlreadyRegisterDependencyWarningMessage = "Service of type [{0}] already registered!";
        public const string NotFoundDependencyErrorMessage = "Service of type [{0}] not found at container!";
    }
    
    public static class Server
    {
        public const string ClientEndpointDefaultName = "Unknown";
        public const string ClientConnectedMessage = "Client with endpoint {0} connected!";
        public const string ClientDisconnectedMessage = "Client with endpoint {0} disconnected!";
        public const string ClientHandlingError = "Client with endpoint {0} fauted with error - {1}";
        public const string ClientDisconnectingError = "Error disconnecting client: {0}";
        
        public const string NotFoundExecutorForRequestErrorMessage = "[{0}] Not found Executor for API: [{1}]";
        public const string FailedExecutingToClientResponseMessage = "[{0}] Error processing message '{1}': by {2}";
        
        public const string SuccessStartingServerMessage = "Server started with address: {0}";
        public const string FailedToStartServerErrorMessage = "Failed to start server: {0}";
        public const string FailedToAcceptClientErrorMessage = "Error accepting client: {0}";
        public const string FailedToHandleClientErrorMessage = "Client {0} error: {1}";
    }
    
    public static class Bootstrapper
    {
        public const string NoCredentialsErrorMessage = "[CRITICAL] No credentials found.";
        public const string CredentialsEmptyDataErrorMessage = "[CRITICAL] Credentials data is empty.";
        public const string NotFoundServerSettingErrorMessage = "[CRITICAL] Server setting is empty.";
        public const string NotFoundDDNSSettingErrorMessage = "[CRITICAL] DDNS setting is empty.";
    }
    
    public static class ServerAPI
    {
        public const string ServerTime = "Server/Time"; // получение времени на сервере (тест)
        public const string ServerPing = "Server/Ping"; // пропинговочка
        public const string DataModelTestModify = "Data/Model/ModifyTestModel"; // тестовое модифицирование модели
    }
}