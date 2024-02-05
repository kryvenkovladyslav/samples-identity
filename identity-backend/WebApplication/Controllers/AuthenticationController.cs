using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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