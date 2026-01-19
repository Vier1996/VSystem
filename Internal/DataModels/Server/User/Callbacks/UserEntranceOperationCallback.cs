using VSystem.Internal.Operations;

namespace VSystem.Internal.DataModels.Server.User;

public record UserEntranceOperationCallback : ServerOperationCallback
{
    public required UserEntranceModel UserModel { get; init; }
}