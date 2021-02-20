using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
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
      var r = new Reservation();

      r.CustomerName = model.CustomerName;
      r.CheckInDate = model.CheckInDate;
      r.CheckOutDate = model.CheckOutDate;
      r.Mobile = model.Mobile;
      r.Email = model.Email;
      r.RoomId = model.RoomId;
      r.Room = model.Room;

      Add(r);
      app.SaveChanges();

      return r;
    }
  }
}
