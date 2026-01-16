using Newtonsoft.Json;
using V_Server.ServerExternal.Services.Data.API.Model.Server;

namespace V_Server.ServerExternal.Services.Data.DataModels;

public class TestServerDataModel : ServerDataModel
{
    [JsonProperty] public int TestNumber { get; private set; }
    
    public void SetTestNumber(int number)
    {
        TestNumber = number;
        MarkAsDirty();
    }
}