using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace VSystem.Internal.Network;

public class NetworkUtility
{
    public static string GetLocalIpAddress()
    {
        var interfaces = NetworkInterface.GetAllNetworkInterfaces()
            .Where(ni => ni.OperationalStatus == OperationalStatus.Up &&
                         ni.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                         (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                          ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
            .OrderByDescending(ni =>
            {
                return ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ? 2 : 1;
            });

        foreach (NetworkInterface ni in interfaces)
        {
            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    byte[] bytes = ip.Address.GetAddressBytes();
                    if (bytes[0] == 192 && bytes[1] == 168)
                    {
                        return ip.Address.ToString();
                    }
                }
            }
        }

        foreach (NetworkInterface ni in interfaces)
        {
            foreach (UnicastIPAddressInformation ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.Address.ToString();
                }
            }
        }

        return "Unknown";
    }
}