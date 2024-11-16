using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NetworkScannerXTech.Data;
using NetworkScannerXTech.Models;
using NetworkScannerXTech.Services;
using System;
using System.Collections.ObjectModel;

namespace NetworkScannerXTech.ViewModels;

public partial class WiFiNetworkViewModel : ObservableObject
{
    private readonly AppDbContext _context;
    private readonly IMessageService _messageService;
    private readonly INetworkScannerService _networkScannerService;

    [ObservableProperty]
    public ObservableCollection<WiFiNetwork> wiFiNetworks = new();
    [ObservableProperty]
    public string bestNetwork = string.Empty;

    public AsyncRelayCommand ScanNetworksAsyncCommand { get; }
    public AsyncRelayCommand SaveToDatabaseAsyncCommand { get; }

    public WiFiNetworkViewModel(AppDbContext context, IMessageService messageService, INetworkScannerService networkScannerService)
    {
        _context = context;
        _messageService = messageService;
        _networkScannerService = networkScannerService;
        WiFiNetworks = new()
        {
            new WiFiNetwork { Bssid = "00:14:22:01:23:45", Ssid = "Network_1", SignalStrength = 45 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:46", Ssid = "Network_2", SignalStrength = 60 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:47", Ssid = "Network_3", SignalStrength = 50 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:48", Ssid = "Network_4", SignalStrength = 70 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:49", Ssid = "Network_5", SignalStrength = 55 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:50", Ssid = "Network_6", SignalStrength = 40 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:51", Ssid = "Network_7", SignalStrength = 65 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:52", Ssid = "Network_8", SignalStrength = 72 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:53", Ssid = "Network_9", SignalStrength = 30 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:54", Ssid = "Network_10", SignalStrength = 80 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:55", Ssid = "Network_11", SignalStrength = 50 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:56", Ssid = "Network_12", SignalStrength = 60 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:57", Ssid = "Network_13", SignalStrength = 45 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:58", Ssid = "Network_14", SignalStrength = 75 },
            new WiFiNetwork { Bssid = "00:14:22:01:23:59", Ssid = "Network_15", SignalStrength = 55 }
        };
        ScanNetworksAsyncCommand = new AsyncRelayCommand(ScanNetworksAsync);
        SaveToDatabaseAsyncCommand = new AsyncRelayCommand(SaveToDatabaseAsync);
    }

    private async Task ScanNetworksAsync()
    {
        WiFiNetworks.Clear();
        var scannedNetworksResult = await _networkScannerService.ScanNetworksAsync();
        if (!scannedNetworksResult.IsSuccess)
        {
            _messageService.ShowError(scannedNetworksResult.Error);
            return;
        }
        foreach (var network in scannedNetworksResult.Value)
        {
            WiFiNetworks.Add(network);
        }
    }

    private async Task SaveToDatabaseAsync()
    {
        var isConfirmed = _messageService.ShowConfirmation("Сохранить в базу данных?", "Сохранение в базу данных");
        if (isConfirmed)
        {
            await _context.AddOrUpdateNetworks(WiFiNetworks);
        }
    }
}
