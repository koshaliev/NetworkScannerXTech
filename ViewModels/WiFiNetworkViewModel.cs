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
    private ObservableCollection<WiFiNetwork> wiFiNetworks = new();
    [ObservableProperty]
    private string bestNetwork = string.Empty;

    public event Action ChangeCursorToWait;
    public event Action ChangeCursorToDefault;

    public AsyncRelayCommand ScanNetworksAsyncCommand { get; }
    public AsyncRelayCommand SaveToDatabaseAsyncCommand { get; }

    public WiFiNetworkViewModel(AppDbContext context, IMessageService messageService, INetworkScannerService networkScannerService)
    {
        _context = context;
        _messageService = messageService;
        _networkScannerService = networkScannerService;
        ScanNetworksAsyncCommand = new AsyncRelayCommand(ScanNetworksAsync);
        SaveToDatabaseAsyncCommand = new AsyncRelayCommand(SaveToDatabaseAsync);
    }

    private async Task ScanNetworksAsync()
    {
        ChangeCursorToWait?.Invoke();

        WiFiNetworks.Clear();
        BestNetwork = string.Empty;

        var scannedNetworksResult = await _networkScannerService.ScanNetworksAsync();
        if (!scannedNetworksResult.IsSuccess)
        {
            _messageService.ShowError(scannedNetworksResult.Error);
            ChangeCursorToDefault?.Invoke();
            return;
        }
        foreach (var network in scannedNetworksResult.Value)
        {
            WiFiNetworks.Add(network);
        }

        var maxSignal = WiFiNetworks.Max(x => x.SignalStrength);
        BestNetwork = WiFiNetworks.FirstOrDefault(x => x.SignalStrength == maxSignal).Ssid;

        ChangeCursorToDefault?.Invoke();
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
