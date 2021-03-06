using GreatFriends.SmartHoltel.APIS.Areas.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GreatFriends.SmartHoltel.Web.Controllers
{
  [Route("api/v1/[controller]")]
  [Produces("application/json")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration config;

    public AuthController(
      SignInManager<IdentityUser> signInManager, 
      UserManager<IdentityUser> userManager, 
      IConfiguration config)
    {
      this.signInManager = signInManager;
      this.userManager = userManager;
      this.config = config;
    }

    [HttpPost]
    public async Task<ActionResult<AuthLoginResponse>> LoginAsync(AuthLoginRequest req) 
    {
      var result = await signInManager.PasswordSignInAsync(req.Username, req.Password, false, false);
      if (!result.Succeeded)
      {
        return Unauthorized();
      }

      var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, req.Username),
            new Claim("randomNumber", (new Random()).Next(0, 100).ToString())
        };

      var identityUser = await userManager.FindByEmailAsync(req.Username);
      var userRoles = await userManager.GetRolesAsync(identityUser);

      foreach (var item in userRoles)
      {
        claims.Add(new Claim(ClaimTypes.Role, item));
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
      var expiration = DateTime.Now.AddDays(10);

      JwtSecurityToken token = new JwtSecurityToken(
        issuer: config["jwt:issuer"], 
        audience: config["jwt:audience"],  
        claims: claims, 
        expires: expiration, 
        signingCredentials: creds);

      // 
      await signInManager.SignOutAsync();

      return new AuthLoginResponse()
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token) 
      };
    }
  }
}
