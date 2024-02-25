using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using WebApplication.Infrastructure.Authentication;
using WebApplication.Models;
using Microsoft.AspNetCore.Identity;
using IdentityDataAccessLayer.Models;
using IdentityUser = IdentityDataAccessLayer.Models.IdentityUser;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class AuthenticationController : ControllerBase
    {
        private readonly string requiredParameter;

        private readonly UserManager<IdentityUser> userManager;

        private readonly SignInManager<IdentityUser> signInManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.requiredParameter = "userName";
        }

        [HttpGet]
        public async Task<bool> IsAuthenticated()
        {
            return await Task.FromResult(this.User.Identity.IsAuthenticated);
        }

        [HttpGet]
        public async Task<ActionResult> SignInWithName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                this.BadRequest();
            }

            var user = await this.userManager.FindByNameAsync(userName);
            await this.signInManager.SignInAsync(user, false);

            return this.Ok();
        }

        [HttpGet]
        public async Task<ActionResult> SignInWithPassword(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                this.BadRequest();
            }

            if (string.IsNullOrEmpty(password))
            {
                this.BadRequest("Incorrect password");
            }


            var user = await this.userManager.FindByNameAsync(userName);
            var result = await this.signInManager.PasswordSignInAsync(userName, password, false,false);

            return this.Ok(result);
        }

        [HttpGet]
        public ActionResult GetClaims()
        {
            var claims = this.User.Claims;
            return this.Ok(claims.Select(claim => new { Type = claim.Type, Value = claim.Value }));
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
        public async Task LogoutWithIdentity()
        {
            await this.signInManager.SignOutAsync();
        }

        [HttpGet]
        public async Task Logout()
        {
            await this.HttpContext.SignOutAsync();
        }
    }
}