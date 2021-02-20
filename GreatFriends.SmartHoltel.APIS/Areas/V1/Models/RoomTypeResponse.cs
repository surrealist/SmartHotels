using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
  public class RoomTypeResponse
  {
    public string Code { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }

    public int RoomCount { get; set; }

    public static RoomTypeResponse FromModel(RoomType m)
    {
      return new RoomTypeResponse()
      {
        Code = m.Code,
        Name = m.Name,
        UnitPrice = m.Price,
        RoomCount = m.Rooms.Count(),
      };
    }
  }
}
