using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GreatFriends.SmartHoltel.Models;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
    public class ReservationRequest
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
        public string Email { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        [Required]
        public int RoomId { get; set; }
       

        public Reservation ToModel() =>
            new Reservation()
            {
                Id = this.Id,
                CheckInDate = this.CheckInDate,
                CheckOutDate = this.CheckOutDate,
                CustomerName = this.CustomerName,
                Email = this.Email,
                Mobile = this.Mobile,
                RoomId = this.RoomId,
                CreatedDate = DateTime.Now
            };

    }
}
