using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WishListWebApp.Models;

namespace WishListWebApp.Data
{
    public class WishListDbContext : IdentityDbContext<ApplicationUser>
    {
        private ILogger _logger { get; }
        private IConfiguration _appConfig { get; }

        public WishListDbContext(
            ILogger<WishListDbContext> logger,
            IConfiguration appConfig)
        {
            this._logger = logger;
            this._appConfig = appConfig;
        }

         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var server = this._appConfig.GetConnectionString("Server");
            var db = this._appConfig.GetConnectionString("DB");
            var username = this._appConfig.GetConnectionString("UserName");
            var password = this._appConfig.GetConnectionString("Password");

            //string connectionString = $"Server={server};Database={db};User Id={username};Password={password};MultipleActiveResultSets=true";
            //Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database = WishListDb;Trusted_Connection = True; MultipleActiveResultSets = true; TrustServerCertificate = True";
             
            optionsBuilder
                .UseSqlServer(connectionString)
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<Wishlist>Wishlist { get; set; }
    }
}
