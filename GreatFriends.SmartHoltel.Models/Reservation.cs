using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GreatFriends.SmartHoltel.Models
{
  public class Reservation
  {
    public int Id { get; set; }

    [Required]
    [StringLength(256)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(256)]
    public string Mobile { get; set; }

    [Required]
    [StringLength(256)]
    public string Email{ get; set; }

    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }

    public virtual Room Room { get; set; }
    public int RoomId { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsCanceled { get; set; } = false;
    public DateTime? CanceledDate { get; set; }
    [StringLength(2000)]
    public string CancelReason { get; set; }

    public void Cancel(DateTime canceledDate, string cancelReason)
    {
      IsCanceled = true;
      CanceledDate = canceledDate;
      CancelReason = cancelReason;
    }
  }
}
