namespace NetworkScannerXTech.Models;
public class WiFiNetwork
{
    public string Bssid { get; set; } = string.Empty; // MAC-адрес
    public string Ssid { get; set; } = string.Empty; // названия сети
    public int SignalStrength { get; set; } // уровень сигнала
}
