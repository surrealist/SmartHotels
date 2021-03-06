using GreatFriends.SmartHoltel.Models;
using System;
using System.Linq;

namespace GreatFriends.SmartHoltel.Services
{
  public class RoomService : ServiceBase<Room>
  {
    public RoomService(App app) : base(app)
    {
      //
    }

    public override Room Add(Room item)
    {
      if (All().Any(x => x.Id == item.Id))
      {
        throw new Exception($"Duplicate room {item.Id}");
      }

      return base.Add(item);
    }
  }
}
