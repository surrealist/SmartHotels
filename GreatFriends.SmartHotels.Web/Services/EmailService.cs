using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace GreatFriends.SmartHotels.Web.Services
{
  public class EmailService : IEmailSender
  {
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
      return Task.FromResult(0);
    }
  }
}
