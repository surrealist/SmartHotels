
using GreatFriends.SmartHoltel.APIS.Areas.V1.Models;
using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services;
using GreatFriends.SmartHoltel.Services.Data;
using GreatFriends.SmartHotel.APIS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Controllers
{
  [Route("api/V1/[controller]")]
  [Produces("application/json")]
  [ApiController]
  public class ReservationsController : ControllerBase
  {
    private readonly App app;

    public ReservationsController(App app)
    {
      this.app = app;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllAsync()
    {
      var items = await app.Reservations.All().ToListAsync();

      var output = items.ConvertAll(ReservationResponse.FromModel);

      return output;
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReservationResponse>> GetByIdAsync(int id)
    {
      var item = await app.Reservations.FindAsync(id);

      if (item == null)
      {
        return NotFound(ErrUtil.CreateDetail($"Reservation id {id} not found"));
      }

      return ReservationResponse.FromModel(item);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReservationResponse>> CreateAsync(ReservationRequest item)
    {
      Reservation model = item.ToModel();
      Reservation reservation = app.Reservations.Create(model);
      await app.SaveChangesAsync();

      return CreatedAtAction(nameof(GetByIdAsync), new { id = reservation.Id }, ReservationResponse.FromModel(reservation));
    } 

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> UpdateAsync(int id, ReservationRequest item)
    {
      if (id != item.Id)
      {
        return BadRequest("Invalid ID");
      }

      var itemRoom = await app.Rooms.FindAsync(id);

      if (itemRoom == null)
      {
        return NotFound(new ProblemDetails() { Title = $"Room id : {id}  not found" });
      }

      var items = await app.Reservations.Query(q =>
                     q.RoomId == item.RoomId &&
                     q.CheckInDate <= item.CheckInDate &&
                     q.CheckOutDate >= item.CheckInDate &&
                     q.IsCanceled == false &&
                     q.Id != item.Id
                 ).ToListAsync();

      if (items.Any())
      {
        return BadRequest(new ProblemDetails
        {
          Title = $"Reservation Duplicate"
        });
      }


      var itemReservation = await app.Reservations.FindAsync(item.Id);

      itemReservation.CheckInDate = item.CheckInDate.Date;
      itemReservation.CheckOutDate = item.CheckOutDate.Date;
      itemReservation.CustomerName = item.CustomerName;
      itemReservation.Email = item.Email;
      itemReservation.Mobile = item.Mobile;
      itemReservation.RoomId = item.RoomId;

      await app.SaveChangesAsync();

      return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReservationResponse>> DeleteAsync(int id, ReservationsDeleteRequest item)
    {

      if (id != item.Id)
      {
        return BadRequest("Invalid ID");
      }

      var itemReservation = await app.Reservations.FindAsync(id);
      if (itemReservation == null)
      {
        return NotFound(new ProblemDetails { Title = $"Reservation id {id} not found" });
      }

      itemReservation.Cancel(item.CanceledDate, item.CancelReason);
      await app.SaveChangesAsync();

      return ReservationResponse.FromModel(itemReservation);
    }

  }
}
