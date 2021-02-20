using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services;
using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GreatFriends.SmartHotel.Tests
{
  public class ReservationServiceTest
  {
    public class Create
    {

      [Fact]
      public void Simple()
      {
        // arrange
        var options = new DbContextOptionsBuilder<AppDb>()
                      .UseInMemoryDatabase($"db-{Guid.NewGuid()}")
                      .Options;
        var db = new AppDb(options);
        var app = new App(db);

        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 3, 20);
        var roomTypeS1 = new RoomType { Code = "S1", Name = "Single bed", Price = 1_000m };
        var room501 = new Room { Id = 501, FloorNo = 5, AreaSquareMeters = 30, RoomTypeCode = "S1" }; //, RoomType = roomTypeS1 };
        app.RoomTypes.Add(roomTypeS1);
        app.Rooms.Add(room501);
        app.SaveChanges();

        var model = new Reservation
        {
          CustomerName = "Alice",
          Mobile = "999",
          Email = "alice@c.com",
          RoomId = 501,
          Room = room501,
          CheckInDate = dt1,
          CheckOutDate = dt2,
        };

        // act
        var r = app.Reservations.Create(model);

        // assert
        Assert.NotNull(r);
        Assert.NotSame(model, r);
        Assert.Equal(501, r.RoomId);
        Assert.Same(room501, r.Room);
        Assert.Equal(dt1, r.CheckInDate);
        Assert.Equal(dt2, r.CheckOutDate);
        Assert.Equal(1, app.Reservations.All().Count());
      }
    }
  }
}
