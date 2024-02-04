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
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using WebApplication.Infrastructure.Authentication;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class AuthenticationController : ControllerBase
    {
        private readonly string requiredParameter;

        public AuthenticationController()
        {
            this.requiredParameter = "userName";
        }

        [HttpGet]
        public async Task SignIn()
        {
            string userName = this.HttpContext.Request.Query[this.requiredParameter];

            if (string.IsNullOrEmpty(userName))
            {
                await this.HttpContext.ChallengeAsync();
            }

            var claim = new Claim(ClaimTypes.Name, userName);
            var identity = new ClaimsIdentity(new[] { claim }, AuthenticationDefaults.CustomScheme);
            await this.HttpContext.SignInAsync(new ClaimsPrincipal(identity));
        }

        [HttpGet]
        public async Task Logout()
        {
            await this.HttpContext.SignOutAsync();
        }
    }
}