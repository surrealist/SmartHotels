using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
  public class RoomRequest
  {
    [Range(0, 999999)]
    public int Id { get; set; }

    [Range(0, 999999)]
    public double AreaSquareMeters { get; set; }

    [Range(-200, 200)]
    public int FloorNo { get; set; }

    [Required]
    [StringLength(450)]
    public string RoomTypeCode { get; set; }

    public Room ToModel() => new Room()
    {
      Id = Id,
      AreaSquareMeters = AreaSquareMeters,
      FloorNo = FloorNo,
      RoomTypeCode = RoomTypeCode
    };
  }
}
