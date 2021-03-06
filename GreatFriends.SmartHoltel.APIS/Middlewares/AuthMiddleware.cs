using GreatFriends.SmartHoltel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Middlewares
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class AuthMiddleware
  {
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext,
      [FromServices] App app,
      [FromServices] UserManager<IdentityUser> userManager)
    {
      if (httpContext.User.Identity.IsAuthenticated)
      {
        var aspnetUser = await userManager.FindByNameAsync(httpContext.User.Identity.Name);

        var roles = httpContext.User
                    .FindAll(ClaimTypes.Role)
                    .Select(x => x.Value).ToArray();
        app.SetCurrentUser(new Guid(aspnetUser.Id), aspnetUser.UserName, roles);
      }

      await _next(httpContext);
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class AuthMiddlewareExtensions
  {
    public static IApplicationBuilder UseAuth(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<AuthMiddleware>();
    }
  }
}
