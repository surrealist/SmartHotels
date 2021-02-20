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
  }
}
