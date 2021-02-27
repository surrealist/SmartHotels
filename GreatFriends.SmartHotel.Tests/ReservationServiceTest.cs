﻿using GreatFriends.SmartHoltel.Models;
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
        var app = new AppBuilder()
                  .WithSingleRoom()
                  .Build();

        var room501 = app.Rooms.Find(501);
        var dt1 = new DateTime(2021, 2, 20);
        var dt2 = new DateTime(2021, 3, 20);
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


    }
  }
}
