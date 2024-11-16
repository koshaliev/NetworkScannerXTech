using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetworkScannerXTech.Data;
using NetworkScannerXTech.Services;
using NetworkScannerXTech.ViewModels;
using System.IO;
using System.Windows;

namespace NetworkScannerXTech;

public partial class App : Application
{
    public IConfiguration Configuration { get; private set; }
    public IServiceProvider Services { get; }
    public new static App Current => (App)Application.Current;

    public App()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Services = ConfigureServices();
        InitializeComponent();
    }

    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddTransient<INetworkScannerService, NetworkScannerService>();
        services.AddTransient<IMessageService, MessageService>();

        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddTransient<WiFiNetworkViewModel>();

        return services.BuildServiceProvider();
    }
}