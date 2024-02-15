using IdentityDataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = IdentityDataAccessLayer.Models.IdentityUser;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserClaimController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserClaimController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUserClaim>>> GetClaimsForUser(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return this.Ok(Enumerable.Empty<IdentityUserClaim>());
            }

            var claims = await this.userManager.GetClaimsAsync(user);

            return this.Ok(claims);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<IdentityUserClaim>>> AddClaimForUser(string userName, string type, string value)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.Ok(Enumerable.Empty<IdentityUserClaim>());
            }

            var claims = await this.userManager.AddClaimAsync(user, new Claim(type, value));

            return this.Ok();
        }

        [HttpDelete]
        public async Task<ActionResult<IEnumerable<IdentityUserClaim>>> DeleteClaimForUser(string userName, string type, string value)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.Ok(Enumerable.Empty<IdentityUserClaim>());
            }

            var result = await this.userManager.RemoveClaimAsync(user, new Claim(type, value));

            return this.Ok(result);
        }
    }
}