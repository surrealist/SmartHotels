using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Models
{
    public class ReservationResponse
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public virtual RoomResponse Room { get; set; }
        public int RoomId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCanceled { get; set; } = false;
        public DateTime? CanceledDate { get; set; }
        public string CancelReason { get; set; }

        public static ReservationResponse FromModel(Reservation item)
        {
            return new ReservationResponse()
            {
                Id = item.Id,
                CanceledDate = item.CanceledDate,
                CheckInDate = item.CheckInDate,
                CancelReason = item.CancelReason,
                CheckOutDate = item.CheckOutDate,
                CreatedDate = item.CreatedDate,
                CustomerName = item.CustomerName,
                Email = item.Email,
                IsCanceled = item.IsCanceled,
                Mobile = item.Mobile,
                Room = RoomResponse.FromModel(item.Room),
                RoomId = item.RoomId,
            };


        }

    }
}
