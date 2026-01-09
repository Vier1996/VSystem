using VSystem.Internal.Constants;
using VSystem.Internal.Server.ServerDataBases;

namespace VSystem.Internal.Server_API.ServerTime.Requests;

[System.Serializable]
public record GetServerTimeRequest : RequestBase
{
    public GetServerTimeRequest()
    {
        RequestApi = AppConstants.ServerAPI.ServerTime;
    }
}