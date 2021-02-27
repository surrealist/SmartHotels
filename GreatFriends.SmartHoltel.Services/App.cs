using GreatFriends.SmartHoltel.Services.Data;
using GreatFriends.SmartHoltel.Services.Exceptions;
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

    public Func<DateTime> Now { get; private set; } = () => DateTime.Now;
    public void SetNow(DateTime now) => Now = () => now;
    public void ResetNow() => Now = () => DateTime.Now;

    public void Throws(AppException ex)
    {
      ex.UserName = "xx";

      throw ex;
    }
  }
}
