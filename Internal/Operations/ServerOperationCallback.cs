namespace VSystem.Internal.Operations;

public record ServerOperationCallback
{
    public required bool IsSuccess { get; init; }
    public required string CallbackMessage { get; init; }
}