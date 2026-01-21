using Newtonsoft.Json;
using VSystem.Internal.ResponseModels;
using VSystem.Server.SystemServer.ServerDataBases;

namespace VSystem.External.Extensions.ResponseModel;

public static class ResponseModelExtension
{
   public static string ToJson(this ResponseModelBase responseModel)
   {
      return JsonConvert.SerializeObject(responseModel, Formatting.Indented);
   }
   
   public static ResponseModelBase ToResponseModelBase(this string json)
   {
      ResponseModelBase model = JsonConvert.DeserializeObject<ResponseModelBase>(json);

      if (model == null)
      {
         Console.WriteLine($"[{nameof(ResponseModelExtension)}] (ERROR) | cannot convert json to {nameof(ResponseDTO)}.\n" +
                           $"Data: \n{json}");
         return null;
      }
      
      return model;
   }
}