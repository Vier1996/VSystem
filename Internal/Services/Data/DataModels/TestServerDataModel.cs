using Newtonsoft.Json;
using VSystem.Internal.Services.Data.API.Model.Server;

namespace VSystem.Internal.Services.Data.DataModels;

public class TestServerDataModel : ServerDataModel
{
    [JsonProperty] public int TestNumber { get; private set; }
    
    public void SetTestNumber(int number)
    {
        TestNumber = number;
        MarkAsDirty();
    }
}