
using GreatFriends.SmartHoltel.APIS.Areas.V1.Models; 
using GreatFriends.SmartHoltel.Models;
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
  [ApiController]
  public class ReservationsController : ControllerBase
  {
    private readonly AppDb db;

    public ReservationsController(AppDb appDb)
    {
      this.db = appDb;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponse>>> GetAllAsync()
    {
      var items = await db.Reservations.ToListAsync();

      var output = items.ConvertAll(ReservationResponse.FromModel);

      return output;
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<ReservationResponse>> GetByIdAsync(int id)
    {
      var item = await db.Reservations.FindAsync(id);

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
      item.CheckInDate = item.CheckInDate.Date;
      item.CheckOutDate = item.CheckOutDate.Date;

      var roomItem = await db.Rooms.FindAsync(item.RoomId);

      if (roomItem == null)
      {
        return NotFound(new ProblemDetails
        {
          Title = $"Room id {item.RoomId} not found"
        });
      }

      var hasOverlapped = db.Reservations.Where(q =>
                    q.RoomId == item.RoomId
                    && !q.IsCanceled
                    && q.CheckInDate <= item.CheckInDate
                    && q.CheckOutDate >= item.CheckInDate
                  ).Any();

      if (hasOverlapped)
      {
        return BadRequest(new ProblemDetails
        {
          Title = $"Reservation Duplicate"
        });
      }

      var newItem = item.ToModel();
      await db.AddAsync(newItem);
      await db.SaveChangesAsync();

      return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, ReservationResponse.FromModel(newItem));
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

      var itemRoom = await db.Rooms.FindAsync(id);

      if (itemRoom == null)
      {
        return NotFound(new ProblemDetails() { Title = $"Room id : {id}  not found" });
      }

      var items = await db.Reservations.Where(q =>
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


      var itemReservation = await db.Reservations.FindAsync(item.Id);

      itemReservation.CheckInDate = item.CheckInDate.Date;
      itemReservation.CheckOutDate = item.CheckOutDate.Date;
      itemReservation.CustomerName = item.CustomerName;
      itemReservation.Email = item.Email;
      itemReservation.Mobile = item.Mobile;
      itemReservation.RoomId = item.RoomId;

      await db.SaveChangesAsync();

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

      var itemReservation = await db.Reservations.FindAsync(id);
      if (itemReservation == null)
      {
        return NotFound(new ProblemDetails { Title = $"Reservation id {id} not found" });
      }

      itemReservation.Cancel(item.CanceledDate, item.CancelReason);
      db.SaveChanges();

      return ReservationResponse.FromModel(itemReservation);
    }

  }
}
