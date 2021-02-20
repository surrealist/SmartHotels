using GreatFriends.SmartHoltel.Services.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.Services
{
  public sealed class App
  {
    internal readonly AppDb db;

    public App(AppDb db)
    {
      this.db = db;

      Rooms = new RoomService(this);
      RoomTypes = new RoomTypeService(this);
      Reservations = new ReservationService(this);
    }

    public RoomService Rooms { get; set; }
    public RoomTypeService RoomTypes { get; set; }
    public ReservationService Reservations { get; set; }

    public int SaveChanges() => db.SaveChanges();
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
  }
}
