using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHoltel.Services.Exceptions
{
  public class UnauthorizedException : AppException
  {
    public UnauthorizedException(string message): base(message)
    {
      //
    }
  }
}
