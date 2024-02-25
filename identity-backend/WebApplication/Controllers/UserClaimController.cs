using IdentityDataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserClaimController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserClaimController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUserClaim>>> GetClaimsForUser(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return this.Ok(Enumerable.Empty<ApplicationUserClaim>());
            }

            var claims = await this.userManager.GetClaimsAsync(user);

            return this.Ok(claims);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ApplicationUserClaim>>> AddClaimForUser(string userName, string type, string value)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.Ok(Enumerable.Empty<ApplicationUserClaim>());
            }

            var claims = await this.userManager.AddClaimAsync(user, new Claim(type, value));

            return this.Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<ApplicationUserClaim>>> DeleteClaimForUser(string userName, string type, string value)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.Ok(Enumerable.Empty<ApplicationUserClaim>());
            }

            var result = await this.userManager.RemoveClaimAsync(user, new Claim(type, value));

            return this.Ok(result);
        }
    }
}