using VSystem.Internal.Constants;

namespace VSystem.Internal.Logging;

public class LoggingService : ILoggingService
{
    public void LogMessage(string message, object? sender = null)
    {
        Console.WriteLine($"(Message) {GetLogString(message, sender)}");
    }

    public void LogWarning(string message, object? sender = null)
    {
        Console.WriteLine($"(Warning) {GetLogString(message, sender)}");
    }

    public void LogError(string message, object? sender = null)
    {
        Console.WriteLine($"(Error) {GetLogString(message, sender)}");
    }

    public void ClearLogs()
    {
        Console.Clear();
    }
    
    private string GetLogString(string message, object? sender)
    {
        return string.Format(
            format: "[{0}]: {1}",
            arg0: sender == null 
                ? AppConstants.Logging.DefaultSenderName 
                : sender.GetType().Name,
            arg1: message
        );
    }
}