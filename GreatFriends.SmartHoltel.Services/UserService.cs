using GreatFriends.SmartHoltel.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GreatFriends.SmartHoltel.Services
{
  public class UserService : ServiceBase<User>
  {
    public UserService(App app) : base(app)
    {
    }
  }
}
