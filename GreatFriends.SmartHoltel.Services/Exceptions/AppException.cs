using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHoltel.Services.Exceptions
{
  public class AppException : ApplicationException
  {
    public string UserName { get; set; } = "guest";
  }
}
