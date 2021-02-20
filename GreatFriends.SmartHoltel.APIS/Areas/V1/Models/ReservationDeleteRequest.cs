using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
    public class ReservationsDeleteRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime CanceledDate { get; set; }

        [StringLength(2000)]
        public string CancelReason { get; set; }

    }
}
