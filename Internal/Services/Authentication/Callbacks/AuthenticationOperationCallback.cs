using VSystem.Internal.Operations;

namespace VSystem.Internal.Services.Authentication;

public record AuthenticationOperationCallback : ServerOperationCallback
{
    public required string SecureToken { get; init; }
}