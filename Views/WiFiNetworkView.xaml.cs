using Microsoft.Extensions.DependencyInjection;
using NetworkScannerXTech.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace NetworkScannerXTech;

public partial class WiFiNetworkView : Window
{
    public WiFiNetworkView()
    {
        InitializeComponent();
        var viewModel = App.Current.Services.GetRequiredService<WiFiNetworkViewModel>();
        viewModel.ChangeCursorToWait += () => Mouse.OverrideCursor = Cursors.Wait;
        viewModel.ChangeCursorToDefault += () => Mouse.OverrideCursor = Cursors.Arrow;
        
        DataContext = viewModel;
    }
}
