using System.Text;

namespace V_Server.ServerExternal.Services.Data.API.Modules;

public interface IDataCrypt
{
    public string Encrypt(string data);
    public string Decrypt(string data);
}

public class DataCrypt : IDataCrypt
{
    private readonly int _cryptoSalt = 33;
        
    public DataCrypt() { }

    public string Encrypt(string data)
    {
        return data;
        return Encoding.UTF8.GetString(RosAlgorithm(Encoding.UTF8.GetBytes(data)));
    }

    public string Decrypt(string data)
    {
        return data;
        return Encoding.UTF8.GetString(RosAlgorithm(Encoding.UTF8.GetBytes(data)));
    }

    private byte[] RosAlgorithm(byte[] bytes)
    {
        for (int i = 0; i < bytes.Length; i++)
            bytes[i] = (byte)(Crypt(bytes[i]));

        return bytes;
    }

    private int Crypt(int input)
    {
        return input ^ _cryptoSalt;
    }
}