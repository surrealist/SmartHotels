using GreatFriends.SmartHoltel.Models;
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

      Users = new UserService(this);

      Rooms = new RoomService(this);
      RoomTypes = new RoomTypeService(this);
      _reservationService = new Lazy<ReservationService>(() => new ReservationService(this));
    }

    public UserService Users { get; }
    public User CurrentUser { get; private set; } = null;
    public bool IsAuthenticated => CurrentUser != null;

    public RoomService Rooms { get; }
    public RoomTypeService RoomTypes { get; }
    public ReservationService Reservations => _reservationService.Value;

    public int SaveChanges() => db.SaveChanges();
    public Task<int> SaveChangesAsync() => db.SaveChangesAsync();

    public Func<DateTime> Now { get; private set; } = () => DateTime.Now;
    public void SetNow(DateTime now) => Now = () => now;
    public void ResetNow() => Now = () => DateTime.Now;
    public DateTime Today() => Now().Date;

    public void SetCurrentUser(Guid id, string username, IEnumerable<string> roles)
    {
      var user = Users.Find(id);
      if (user == null)
      {
        user = new User
        {
          Id = id,
          UserName = username,
          CreatedDate = Now(),
          Note = null
        };
        Users.Add(user);
        SaveChanges();
      }

      user.Roles = roles;
      CurrentUser = user;
    }

    public void ClearCurrentUser()
    {
      CurrentUser = null;
    }

    public void Throws(AppException ex)
    {
      ex.UserId = CurrentUser?.Id;
      ex.UserName = CurrentUser?.UserName;

      throw ex;
    }
  }
}
