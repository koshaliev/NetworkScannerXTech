using Microsoft.EntityFrameworkCore;
using NetworkScannerXTech.Models;
using Npgsql;

namespace NetworkScannerXTech.Data;
public class AppDbContext : DbContext
{
    public DbSet<WiFiNetwork> WiFiNetworks { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WiFiNetwork>()
            .HasKey(w => w.Bssid);
    }

    public async Task AddOrUpdateNetworks(IEnumerable<WiFiNetwork> networks)
    {
        using (var transaction = await Database.BeginTransactionAsync())
        {
            try
            {
                foreach (var network in networks)
                {
                    var sql = @"
                        INSERT INTO public.""WiFiNetworks"" (""Bssid"", ""Ssid"", ""SignalStrength"")
                        VALUES (@Bssid, @Ssid, @SignalStrength)
                        ON CONFLICT (""Bssid"")
                        DO UPDATE SET
                            ""Ssid"" = EXCLUDED.""Ssid"",
                            ""SignalStrength"" = EXCLUDED.""SignalStrength"";
                    ";

                    await Database.ExecuteSqlRawAsync(sql,
                        new NpgsqlParameter("@Bssid", network.Bssid),
                        new NpgsqlParameter("@Ssid", network.Ssid),
                        new NpgsqlParameter("@SignalStrength", network.SignalStrength)
                    );
                }

                await SaveChangesAsync();
                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }
    }
}


