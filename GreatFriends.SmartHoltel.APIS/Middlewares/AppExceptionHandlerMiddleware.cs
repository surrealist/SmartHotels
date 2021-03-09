using GreatFriends.SmartHoltel.APIS.Models;
using GreatFriends.SmartHoltel.Services.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.APIS.Middlewares
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class AppExceptionHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public AppExceptionHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
      try
      {
        await _next(httpContext);
      }
      catch (Exception ex)
      {
        await HandleExceptionAsync(httpContext, ex);
      }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
    {
      ApiError obj;

      switch (ex)
      {
        case ReservationException ex2:
          httpContext.Response.StatusCode = 500;
          obj = new ApiError
          {
            StatusCode = httpContext.Response.StatusCode,
            Message = ex2.Message
          };
          break;

        case UnauthorizedException ex2:
          httpContext.Response.StatusCode = 401;
          obj = new ApiError
          {
            StatusCode = httpContext.Response.StatusCode,
            Message = ex2.Message
          };
          break;

        default:
          httpContext.Response.StatusCode = 500;
          obj = new ApiError
          {
            StatusCode = httpContext.Response.StatusCode,
            Message = ex.Message
          };
          break;
      }

      await httpContext.Response.WriteAsJsonAsync(obj);
    }
  }

  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class AppExceptionHandlerMiddlewareExtensions
  {
    public static IApplicationBuilder UseAppExceptionHandler(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<AppExceptionHandlerMiddleware>();
    }
  }
}
