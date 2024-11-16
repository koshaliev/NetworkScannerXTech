using Microsoft.Extensions.DependencyInjection;
using NetworkScannerXTech.ViewModels;
using System.Windows;

namespace NetworkScannerXTech;

public partial class WiFiNetworkView : Window
{
    public WiFiNetworkView()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetRequiredService<WiFiNetworkViewModel>();
    }
}
