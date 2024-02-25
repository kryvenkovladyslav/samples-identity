using IdentityDataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public sealed class UserRoleController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRoleController(UserManager<ApplicationUser> userManager)        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost]
        public async Task<ActionResult> AddUserToRole(string userName, string roleName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if(user == null)
            {
                return this.BadRequest("User does not exist");
            }

            var result = await this.userManager.AddToRoleAsync(user, roleName);

            if(!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Created("~/api/UserRole/AddUserToRle", userName);    
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveUserFromRole(string userName, string roleName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.BadRequest("User does not exist");
            }

            var result = await this.userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(userName);
        }

        [HttpGet]
        public async Task<ActionResult> GetRolesForUser(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.BadRequest("User does not exist");
            }

            var result = await this.userManager.GetRolesAsync(user);

            return this.Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsersInRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return this.BadRequest($"{roleName} is empty");
            }

            var result = await this.userManager.GetUsersInRoleAsync(roleName);

            return this.Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> IsInRole(string userName, string roleName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.BadRequest("User does not exist");
            }

            var result = await this.userManager.IsInRoleAsync(user, roleName);

            return this.Ok(result);
        }
    }
}