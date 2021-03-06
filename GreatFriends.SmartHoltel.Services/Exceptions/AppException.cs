using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHoltel.Services.Exceptions
{
  public class AppException : ApplicationException
  {

    public AppException()
    {
      //
    }

    public AppException(string message): base(message)
    {
      //
    }

    public Guid? UserId { get; set; }
    public string UserName { get; set; } = "guest";
  }
}
