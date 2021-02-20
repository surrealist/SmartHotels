using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Controllers
{
  [Route("api/v1/[controller]")]
  [ApiController]
  public class InfoController : ControllerBase
  {
    private readonly IConfiguration config;

    public InfoController(IConfiguration config)
    {
      this.config = config;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public ActionResult<string> GetInfo() //ActionResult ไม่จำเป็นต้องเอา ok ครอบส่งค่าตรงๆออกไปได้เลย
    {
      if (config["Pin"] == null)
      {
        return NotFound();
      }

      return config["Pin"];
    }
  }
}
