using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; 


namespace GreatFriends.SmartHotel.Migrations
{
  public class AppDbFactory : IDesignTimeDbContextFactory<AppDb>
  {
    public AppDb CreateDbContext(string[] args)
    {
      var config = new ConfigurationBuilder()
                   .AddJsonFile($"appsettings.json")
                   .Build();

      var options = new DbContextOptionsBuilder<AppDb>() 
                    //.UseSqlServer(config.GetConnectionString(nameof(AppDb)), 
                    .UseSqlite(config.GetConnectionString(nameof(AppDb)),
                      x => x.MigrationsAssembly("GreatFriends.SmartHotel.Migrations")) 
                    .Options;

      var db = new AppDb(options);

      return db;
    }
  }
}
