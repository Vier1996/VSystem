using Newtonsoft.Json;
using VSystem.Internal.RequestArgs;

namespace VSystem.External.Extensions.RequestArgs;

public static class RequestArgsExtension
{
    public static string ToJson(this RequestArgsBase requestArgs)
    {
        return JsonConvert.SerializeObject(requestArgs, Formatting.Indented);
    }
   
    public static RequestArgsBase FromJson(this string json)
    {
        RequestArgsBase requestArgs = JsonConvert.DeserializeObject<RequestArgsBase>(json);

        if (requestArgs == null)
        {
            Console.WriteLine($"[{nameof(RequestArgsExtension)}] (ERROR) | cannot convert json to {nameof(RequestArgsBase)}.\n" +
                              $"Data: \n{json}");
            return null;
        }
      
        return requestArgs;
    } 
}