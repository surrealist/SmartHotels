using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHotel.APIS
{
  public static class ErrUtil
  {

    public static ProblemDetails CreateDetail(string title = null, string detail = null)
    {
      return new ProblemDetails() { Title = title, Detail = detail };
    }

  }
}
