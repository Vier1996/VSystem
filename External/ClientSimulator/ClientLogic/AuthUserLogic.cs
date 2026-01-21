using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using VSystem.External.Extensions.RequestArgs;
using VSystem.External.Extensions.ResponseModel;
using VSystem.Internal.Constants;
using VSystem.Internal.RequestArgs.Auth;
using VSystem.Internal.RequestArgs.Register;
using VSystem.Internal.ResponseModels.Auth;
using VSystem.Internal.ResponseModels.Register;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.External.ClientSimulator.ClientLogic;

public class AuthUserLogic : IClientLogicVariant
{
    public async Task<string> Run(TcpClient client)
    {
        AuthUserRequestArgs requestArgs = new()
        {
            Login = "MrVier",
            Password = "Qwerty69",
        };

        string json = JsonConvert.SerializeObject(new RequestDTO()
        {
            RequestApi = AppConstants.ServerAPI.Authentication.AuthenticateUser,
            RequestArgs = requestArgs.ToJson(),
        });

        byte[] data = Encoding.UTF8.GetBytes(json);

        NetworkStream stream = client.GetStream();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

        await stream.WriteAsync(data, 0, data.Length, cts.Token);

        byte[] buffer = new byte[2048];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
        string responseData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

        ResponseDTO responseDto = JsonConvert.DeserializeObject<ResponseDTO>(
            value: responseData,
            settings: new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            })!;
        
        AuthUserResponse response = JsonConvert.DeserializeObject<AuthUserResponse>(
            value: responseDto.Content,
            settings: new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
            })!;
        
        return 
            "----- (AuthUserRequest) ------\n" +
            $"Status: {responseDto.StatusCode.ToString()}\n" + 
            $"Response: \n{response.ToJson()}\n" +
            "-----------------------------------"
            ;
    }
}