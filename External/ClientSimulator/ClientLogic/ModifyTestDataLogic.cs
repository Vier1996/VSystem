using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using VSystem.Internal.Constants;
using VSystem.Internal.ServerInfrastructure.Server_API.DataModels;
using VSystem.Internal.ServerInfrastructure.Server_API.Ping;
using VSystem.Internal.ServerInfrastructure.Server.ServerDataBases;

namespace VSystem.External.ClientSimulator.ClientLogic;

public class ModifyTestDataLogic : IClientLogicVariant
{
    public async Task<string> Run(TcpClient client)
    {
        string json = JsonConvert.SerializeObject(new RequestDTO()
        {
            RequestApi = AppConstants.ServerAPI.DataModelTestModify,
            RequestArgs = string.Empty,
        });
            
        byte[] data = Encoding.UTF8.GetBytes(json);

        NetworkStream stream = client.GetStream();
            
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            
        await stream.WriteAsync(data, 0, data.Length, cts.Token);

        byte[] buffer = new byte[2048];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
        string responseData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            
        ResponseDTO responseDto = JsonConvert.DeserializeObject<ResponseDTO>(responseData, settings: new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        })!;
            
        ModifyServerTestModelResponseData responsePingData = JsonConvert.DeserializeObject<ModifyServerTestModelResponseData>(responseDto.Message, settings: new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
        })!;
            
        return responsePingData.NewValue.ToString();
    }
}