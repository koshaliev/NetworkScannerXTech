﻿using ManagedNativeWifi;
using NetworkScannerXTech.Models;
using NetworkScannerXTech.Shared;
using System.Collections.Generic;
using System.Text;

namespace NetworkScannerXTech.Services;
public class NetworkScannerService : INetworkScannerService
{
    public async Task<Result<IEnumerable<WiFiNetwork>>> ScanNetworksAsync()
    {
        try
        {
            await NativeWifi.ScanNetworksAsync(TimeSpan.FromSeconds(1));
            var scanResult = NativeWifi.EnumerateBssNetworks();
            
            var scannedNetworks = scanResult.Select(n => 
                new WiFiNetwork { 
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