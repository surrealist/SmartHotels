using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services;
using GreatFriends.SmartHoltel.Services.Data;
using GreatFriends.SmartHoltel.Services.Exceptions;
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
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25); // 5 nights
        var model = new Reservation
        {
          CustomerName = "Alice",
          Mobile = "999",
          Email = "alice@c.com",
          RoomId = room501.Id,
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

      public static IEnumerable<object[]> InvalidDateData(string startDate, int days)
      {
        var d = DateTime.Parse(startDate);
        for (int i = 0; i < days; i++)
        {
          yield return new[] { startDate, d.ToString("yyyy-MM-dd") };

          d = d.AddDays(-1);
        }
      }

      [Theory]
      [MemberData(nameof(InvalidDateData), "2021-03-20", 5)]
      //[InlineData("2021-03-20", "2021-03-20")]
      //[InlineData("2021-03-20", "2021-03-19")]
      public void InvalidDate_Error(string checkIn, string checkOut)
      {
        var app = new AppBuilder()
                .WithSingleRoom()
                .Build();

        var room501 = app.Rooms.Find(501);
        var checkInDate = DateTime.Parse(checkIn);
        var checkOutDate = DateTime.Parse(checkOut);
        var model = new Reservation
        {
          CustomerName = "Alice",
          Mobile = "999",
          Email = "alice@c.com",
          RoomId = room501.Id,
          Room = room501,
          CheckInDate = checkInDate,
          CheckOutDate = checkOutDate,
        };

        var ex = Assert.ThrowsAny<Exception>(() =>
        {
          var r = app.Reservations.Create(model);
        });

        // assert 
        Assert.Equal("Invalid checkin or checkout date", ex.Message);
      }

      [Fact]
      public void OverlappingCase1()
      {
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25);
        var alice = CreateReservation("Alice", room501, dt1, dt2);
        var r1 = app.Reservations.Create(alice);

        var dt3 = new DateTime(2021, 2, 10);
        var dt4 = new DateTime(2021, 2, 21);
        var bob = CreateReservation("Bob", room501, dt3, dt4);

        // act
        var ex = Assert.Throws<ReservationException>(() =>
        {
          var r2 = app.Reservations.Create(bob);
        });

        Assert.Equal("Overlapping", ex.Reason);
        Assert.Equal("Bob", ex.CustomerName);
        Assert.Equal(501, ex.RoomId);
      }

      private Reservation CreateReservation(
          string customerName,
          Room room,
          DateTime checkIn, DateTime checkOut)
        => new Reservation
        {
          CustomerName = customerName,
          Mobile = "999",
          Email = $"{customerName}@hotel.com",
          RoomId = room.Id,
          Room = room,
          CheckInDate = checkIn,
          CheckOutDate = checkOut,
        };
    }
  }
}
