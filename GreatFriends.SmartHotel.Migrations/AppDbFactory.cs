using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    .UseSqlServer(config.GetConnectionString(nameof(AppDb)), 
                      x => x.MigrationsAssembly("GreatFriends.SmartHotel.Migrations")) 
                    .Options;

      var db = new AppDb(options);

      return db;
    }
  }
}
