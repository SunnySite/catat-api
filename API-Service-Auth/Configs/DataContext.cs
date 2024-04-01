using API_Service_Auth.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace API_Service_Auth.Configs;

public class DataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();
            var connectionString = configuration.GetConnectionString("Main");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    public DbSet<UserMst> UserMsts { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
}

