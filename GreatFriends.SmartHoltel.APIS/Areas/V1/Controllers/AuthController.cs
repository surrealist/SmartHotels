using GreatFriends.SmartHoltel.Models;
using GreatFriends.SmartHoltel.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Areas.V1.Controllers
{
  [Produces("application/json")]
  [Route("api/v1/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly App app;

    public AuthController(App app)
    {
      this.app = app;
    }

    [HttpGet]
    public ActionResult<User> GetInfo()
    {
      return app.CurrentUser;
    }
  }
}
