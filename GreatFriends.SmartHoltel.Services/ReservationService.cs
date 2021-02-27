using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreatFriends.SmartHoltel.Services
{
  public class ReservationService : ServiceBase<Reservation>
  {
    public ReservationService(App app) : base(app)
    {
    }

    public Reservation Create(Reservation model)
    {
      if (model.CheckOutDate <= model.CheckInDate)
      {
        throw new Exception("Invalid checkin or checkout date");
      }

      var activeReservations = app.Reservations.Query(q =>
        q.RoomId == model.RoomId
        && !q.IsCanceled);

      var hasOverlapped1 = activeReservations.Any(q =>
        q.CheckInDate <= model.CheckInDate
        && q.CheckOutDate >= model.CheckInDate
      );

      var hasOverlapped2 = activeReservations.Any(q =>
        q.CheckInDate <= model.CheckOutDate
        && q.CheckOutDate >= model.CheckOutDate
      );

      var hasOverlapped3 = activeReservations.Any(q =>
        q.CheckInDate >= model.CheckInDate
        && q.CheckOutDate <= model.CheckOutDate
      );

      if (hasOverlapped1 || hasOverlapped2 || hasOverlapped3)
      {
        throw new Exception("Overlapping Reservations");
      }

      var r = new Reservation
      {
        CustomerName = model.CustomerName,
        CheckInDate = model.CheckInDate,
        CheckOutDate = model.CheckOutDate,
        Mobile = model.Mobile,
        Email = model.Email,
        RoomId = model.RoomId,
        Room = model.Room
      };

      Add(r);
      app.SaveChanges();

      return r;
    }
  }
}
