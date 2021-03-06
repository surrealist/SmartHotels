using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services;
using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHotel.Tests
{
  internal class AppBuilder
  {
    private App app;

    public AppBuilder()
    {
      var options = new DbContextOptionsBuilder<AppDb>()
                     .UseInMemoryDatabase($"db-{Guid.NewGuid()}")
                     .Options;
      var db = new AppDb(options);
      app = new App(db);
    }

    public AppBuilder SetNow(DateTime now)
    {
      app.SetNow(now);
      return this;
    }
     
    public AppBuilder SignedInAsUser()
    {
      app.SetCurrentUser(Guid.NewGuid(), "Alice", new[] { "user" });
      return this;
    }

    public AppBuilder NotSignedIn()
    {
      app.ClearCurrentUser();
      return this;
    }

    public AppBuilder WithSingleRoom()
    {
      var roomTypeS1 = new RoomType { Code = "S1", Name = "Single bed", Price = 1_000m };
      var room501 = new Room
      {
        Id = 501,
        FloorNo = 5,
        AreaSquareMeters = 30,
        RoomTypeCode = "S1",
        RoomType = roomTypeS1
      };
      app.RoomTypes.Add(roomTypeS1);
      app.Rooms.Add(room501);
      app.SaveChanges();

      return this;
    }
    
    public App Build() => app;
  }
}
