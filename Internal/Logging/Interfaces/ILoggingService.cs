namespace VSystem.Internal.Logging;

public interface ILoggingService
{
    public void LogMessage(string message, object? sender = null);
    public void LogWarning(string message, object? sender = null);
    public void LogError(string message, object? sender = null);
    
    public void ClearLogs();
}