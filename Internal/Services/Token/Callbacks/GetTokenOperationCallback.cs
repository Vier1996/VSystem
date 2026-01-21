using VSystem.Internal.DataModels.Server.Token;
using VSystem.Internal.Operations;

namespace VSystem.Internal.Services.Token;

public record GetTokenOperationCallback : ServerOperationCallback
{
    public required SecureTokenInfoModel TokenInfo { get; init; }
}