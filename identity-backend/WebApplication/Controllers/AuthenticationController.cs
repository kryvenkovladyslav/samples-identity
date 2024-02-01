using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
using System;
using WebApplication.Models;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class AuthenticationController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public ActionResult AuthenticateUser([FromBody] UserAuthentication user)
        {
            if (user == null)
            {
                return this.BadRequest("User is empty");
            }

            var userName = new Claim(ClaimTypes.Name, user.UserName);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(userName);
            var principal = new ClaimsPrincipal(identity);

            this.HttpContext.SignInAsync(principal);

            return this.Ok("Authenticated");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Logout()
        {
            this.HttpContext.SignOutAsync();
            return this.Ok("Done");
        }
    }
}