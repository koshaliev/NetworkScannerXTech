using NetworkScannerXTech.Models;
using NetworkScannerXTech.Shared;

namespace NetworkScannerXTech.Services;
public interface INetworkScannerService
{
    public Task<Result<IEnumerable<WiFiNetwork>>> ScanNetworksAsync();
}
