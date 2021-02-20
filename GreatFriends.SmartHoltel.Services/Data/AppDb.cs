using GreatFriends.SmartHoltel.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHoltel.Services.Data
{
  // -s คือการบอกตำแหน่งของ startup file
  //todo  dotnet ef migrations add update01 -s ..\GreatFriends.SmartHotel.APIs\GreatFriends.SmartHotel.APIs.csproj -o Data\Migrations
  //todo  dotnet ef database update -s ..\GreatFriends.SmartHotel.APIs\GreatFriends.SmartHotel.APIs.csproj

  public class AppDb : DbContext
  {

    //public AppDb()
    //{
    //  //
    //}

    public AppDb(DbContextOptions<AppDb> options) : base(options)
    {
      //
    }

    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomType> RoomTypes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //  if (!optionsBuilder.IsConfigured)
    //  {
    //    optionsBuilder.UseSqlServer("Data Source=(local)\\sqlexpress;Initial Catalog=SmartHotel_Thitipong;Integrated Security=True;multipleactiveresultsets=true");
    //  }
    //}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<RoomType>().Property(x => x.Price)
        .HasColumnType("decimal")
        .HasPrecision(18, 2);
    }
  }
}
