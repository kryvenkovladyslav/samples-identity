using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        public UserController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApplicationUser>> GetAllUsers()
        {
            return this.Ok(this.userManager.Users.ToList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllUsersNames()
        {
            return this.Ok(this.userManager.Users.OrderBy(user => user.UserName).Select(user => user.UserName).ToList());
        }

        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> FindByEmail(string email)
        {
            return this.Ok(await this.userManager.FindByEmailAsync(email));
        }

        [HttpGet]
        public async Task<ActionResult<ApplicationUser>> FindByName(string name)
        {
            return this.Ok(await this.userManager.FindByNameAsync(name));
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> AddUser(ApplicationUser user)
        {
            // This part of code can be deleted because UserManager takes over validation process

            /*var user = await this.userManager.FindByNameAsync(name);

            if(user != null)
            {
                return this.BadRequest();
            }*/

            var creationResult = await this.userManager.CreateAsync(user);

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Created("~api/User/AddUser", user.UserName);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteUser(string name)
        {
            var user = await this.userManager.FindByNameAsync(name);

            if (user == null)
            {
                return this.BadRequest();
            }

            var creationResult = await this.userManager.DeleteAsync(user);

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Ok(name);
        }

        [HttpPut]
        public async Task<ActionResult<ApplicationUser>> UpdateUser(string name, string newUserName)
        {
            var user = await this.userManager.FindByNameAsync(name);

            if (user == null)
            {
                return this.BadRequest();
            }

            var creationResult = await this.userManager.UpdateAsync(user);

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Ok(name);
        }

    }
}