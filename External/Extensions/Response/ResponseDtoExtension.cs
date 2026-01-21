using Newtonsoft.Json;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.External.Extensions.ResponseModel;

public static class ResponseDtoExtension
{
    public static string ToJson(this ResponseDTO responseDto)
    {
        return JsonConvert.SerializeObject(responseDto, Formatting.Indented);
    }
   
    public static ResponseDTO FromJson(this string json)
    {
        ResponseDTO dto = JsonConvert.DeserializeObject<ResponseDTO>(json);

        if (dto == null)
        {
            Console.WriteLine($"[{nameof(ResponseModelExtension)}] (ERROR) | cannot convert json to {nameof(ResponseDTO)}.\n" +
                              $"Data: \n{json}");
            return null;
        }
      
        return dto;
    }
}