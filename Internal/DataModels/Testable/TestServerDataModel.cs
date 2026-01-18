using Newtonsoft.Json;
using VSystem.Internal.Services.Data.Model.Server;

namespace VSystem.Internal.DataModels.Testable;

[Serializable]
public record TestServerDataModel : ServerDataModel
{
    [JsonProperty] public int TestNumber { get; private set; }
    [JsonProperty] private int _testNumber1;
    
    public void SetTestNumber(int number)
    {
        TestNumber = number;
        _testNumber1 = number + 5;

        IsDirty = true;
    }
}