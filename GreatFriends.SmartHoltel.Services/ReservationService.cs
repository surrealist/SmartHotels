using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services.Exceptions;
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
      if (!app.IsAuthenticated) {
        app.Throws(new UnauthorizedException("Anonymous cannot make a reservation"));
      }

      if (model.CheckInDate < app.Today())
      {
        app.Throws(new ReservationException(model.CustomerName, model.RoomId,
          "Cannot make a reservation in the past"));
      }

      if (model.CheckOutDate <= model.CheckInDate)
      {
        app.Throws(new ReservationException(model.CustomerName, model.RoomId, "Invalid checkin or checkout date"));
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
        app.Throws(new ReservationException(model.CustomerName, model.RoomId, "Overlapping"));
      }

      var r = new Reservation
      {
        CustomerName = model.CustomerName,
        CheckInDate = model.CheckInDate,
        CheckOutDate = model.CheckOutDate,
        Mobile = model.Mobile,
        Email = model.Email,
        Room = model.Room,
        RoomId = model.RoomId,
        CreatedDate = app.Now(),
        IsCanceled = false,
        CanceledDate = null,
        CancelReason = null,
      };

      Add(r);
      app.SaveChanges();

      return r;
    }
  }
}
