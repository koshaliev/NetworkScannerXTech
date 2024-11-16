using ManagedNativeWifi;
using NetworkScannerXTech.Models;
using NetworkScannerXTech.Shared;

namespace NetworkScannerXTech.Services;
public class NetworkScannerService : INetworkScannerService
{
    public async Task<Result<IEnumerable<WiFiNetwork>>> ScanNetworksAsync()
    {
        try
        {
            await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(1)); // производим ручной поиск, чтобы "запустить" wifi-модуль.
            var scanResult = NativeWifi.EnumerateBssNetworks(); // получаем список сетей

            var scannedNetworks = scanResult.Select(n =>
                new WiFiNetwork
                {
                    Bssid = n.Bssid.ToString(),
                    Ssid = n.Ssid.ToString(),
                    SignalStrength = n.LinkQuality
                }).ToList();
            return Result<IEnumerable<WiFiNetwork>>.Success(scannedNetworks);
        }
        catch (System.Reflection.TargetInvocationException ex)
        {
            if (ex.InnerException is System.ComponentModel.Win32Exception)
            {
                return Result<IEnumerable<WiFiNetwork>>.Failure("WiFi-модуль не найден.");
            }
            return Result<IEnumerable<WiFiNetwork>>.Failure(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<WiFiNetwork>>.Failure(ex.ToString());
        }
    }
}
