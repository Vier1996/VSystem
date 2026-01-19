using VSystem.Internal.Services.Data.Model.Server;
using VSystem.Internal.Services.Data.Model.User;

namespace VSystem.Internal.Services.Data;

public interface IDataService : IDisposable
{
    public void SaveAllForce();
    
    public TModel ResolveServerData<TModel>() where TModel : ServerDataModel;
    public TModel ResolveUserData<TModel>(string userToken) where TModel : UserDataModel;
}
