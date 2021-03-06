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
using Should;

namespace GreatFriends.SmartHotel.Tests
{
  public class ReservationServiceTest
  {
    public class Create
    {

      [Fact]
      public void SimpleCase()
      {
        DateTime now = new DateTime(2021, 2, 1, 10, 30, 5);
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .SetNow(now)
                  .SignedInAsUser()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25); // 5 nights
        var alice = CreateReservation("Alice", room501, dt1, dt2);

        // act
        var r = app.Reservations.Create(alice);

        // assert
        Assert.NotNull(r);
        Assert.NotSame(alice, r);
        Assert.Equal(501, r.RoomId);
        Assert.Same(room501, r.Room);
        Assert.Equal(dt1, r.CheckInDate);
        Assert.Equal(dt2, r.CheckOutDate);
        Assert.Equal(now, r.CreatedDate);
        Assert.Equal(1, app.Reservations.All().Count());
      }

      [Fact]
      public void MakeReservationInThePast_ThrowsEx()
      {
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .SetNow(new DateTime(2021, 2, 16)) // 16 Feb
                  .SignedInAsUser()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 14);
        var dt2 = new DateTime(2021, 2, 15);
        var input = CreateReservation("Alice", room501, dt1, dt2);

        var ex = Assert.Throws<ReservationException>(() =>
        {
          var reservation = app.Reservations.Create(input);
        });

        ex.RoomId.ShouldEqual(501);
        ex.Reason.ShouldEqual("Cannot make a reservation in the past");
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
      public void InvalidDate_Error(string checkIn, string checkOut)
      {
        var app = new AppBuilder()
                .WithSingleRoom()
                .SignedInAsUser()
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

        var ex = Assert.Throws<ReservationException>(() =>
        {
          var r = app.Reservations.Create(model);
        });

        // assert 
        Assert.Equal("Invalid checkin or checkout date", ex.Reason);
      }

      [Theory]
      [InlineData("2021-02-10", "2021-02-21")]
      [InlineData("2021-02-10", "2021-02-22")]
      [InlineData("2021-02-10", "2021-02-25")]
      [InlineData("2021-02-10", "2021-02-26")]
      [InlineData("2021-02-10", "2021-02-27")]
      [InlineData("2021-02-20", "2021-02-22")]
      [InlineData("2021-02-21", "2021-02-22")]
      [InlineData("2021-02-22", "2021-02-27")]
      public void OverlappingCase1(string checkIn, string checkOut)
      {
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .SetNow(new DateTime(2021, 2, 1))
                  .SignedInAsUser()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25);
        var alice = CreateReservation("Alice", room501, dt1, dt2);
        var r1 = app.Reservations.Create(alice);

        var dt3 = DateTime.Parse(checkIn);
        var dt4 = DateTime.Parse(checkOut);
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

      [Fact]
      public void Anonymous_CannotMakeReservation()
      {
        DateTime now = new DateTime(2021, 2, 1, 10, 30, 5);
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .SetNow(now)
                  .NotSignedIn()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25); // 5 nights
        var alice = CreateReservation("Alice", room501, dt1, dt2);

        Assert.Throws<UnauthorizedException>(() =>
        {
          var r = app.Reservations.Create(alice);
        });

      }

      [Fact]
      public void User_CanMakeReservation()
      {
        DateTime now = new DateTime(2021, 2, 1, 10, 30, 5);
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .SetNow(now)
                  .SignedInAsUser()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 2, 25); // 5 nights
        var alice = CreateReservation("Alice", room501, dt1, dt2);

        // act
        var r = app.Reservations.Create(alice);

        Assert.NotNull(r);
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
