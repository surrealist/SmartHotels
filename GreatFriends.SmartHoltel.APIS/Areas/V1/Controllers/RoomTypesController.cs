using GreatFriends.SmartHoltel.APIS.Areas.V1.Models;
using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class RoomTypesController : ControllerBase
  {
    private readonly AppDb db;

    public RoomTypesController(AppDb db)
    {
      this.db = db;
    }


    [HttpGet]
    public ActionResult<IEnumerable<RoomType>> GetAll()
    {
      var items = db.RoomTypes.OrderBy(x => x.Name).ToList();

      return items;
    }

    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public ActionResult<RoomType> GetByCode(string code)
    {
      var item = db.RoomTypes.Find(code);
      if (item == null)
      {
        return NotFound(new ProblemDetails { Title = $"Room type {code} not found" });
      }

      return item;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesDefaultResponseType]
    public ActionResult<RoomType> Create(RoomTypeRequest item)
    {
      var roomType = new RoomType();
      roomType.Code = item.Code.ToUpper();
      roomType.Name = item.Name;
      roomType.Price = item.UnitPrice;


      db.Add(roomType);
      db.SaveChanges();

      return CreatedAtAction(nameof(GetByCode), new { code = roomType.Code }, roomType);

    }


    [HttpPut("{code}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public ActionResult Update(string code, RoomTypeRequest item)
    {
      if (code != item.Code)
      {
        return BadRequest();
      }

      var roomType = db.RoomTypes.Find(code);
      if (roomType == null)
      {
        return NotFound(new ProblemDetails { Title = $"Room type {code} not found" });
      }

      roomType.Name = item.Name;
      roomType.Price = item.UnitPrice;
      db.SaveChanges();

      return NoContent();
    }


    [HttpDelete("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public ActionResult<RoomType> Delete(string code)
    {
      var roomType = db.RoomTypes.Find(code);
      if (roomType == null)
      {
        return NotFound(new ProblemDetails { Title = $"Room type {code} not found" });
      }

      db.Remove(roomType);
      db.SaveChanges();

      return Ok(roomType);
    }


  }
}
