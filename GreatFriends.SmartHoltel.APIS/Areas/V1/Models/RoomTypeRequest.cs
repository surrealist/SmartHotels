using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
    public class RoomTypeRequest
    {

        [Required]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Range(0, 99_999)]
        public decimal UnitPrice { get; set; }


    }
}
