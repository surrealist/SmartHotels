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
    private Lazy<ReservationService> _reservationService;

    public App(AppDb db)
    {
      this.db = db;

      Rooms = new RoomService(this);
      RoomTypes = new RoomTypeService(this);
      _reservationService = new Lazy<ReservationService>(() => new ReservationService(this));
    }

    public RoomService Rooms { get; }
    public RoomTypeService RoomTypes { get; }
    public ReservationService Reservations => _reservationService.Value;

    public int SaveChanges() => db.SaveChanges();
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();
  }
}
