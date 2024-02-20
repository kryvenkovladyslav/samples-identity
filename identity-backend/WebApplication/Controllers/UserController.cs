using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityUser = IdentityDataAccessLayer.Models.IdentityUser;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public sealed class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        
        public UserController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<IdentityUser>> GetAllUsers()
        {
            return this.Ok(this.userManager.Users.ToList());
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllUsersNames()
        {
            return this.Ok(this.userManager.Users.OrderBy(user => user.UserName).Select(user => user.UserName).ToList());
        }

        [HttpGet]
        public async Task<ActionResult<IdentityUser>> FindByEmail(string email)
        {
            return this.Ok(await this.userManager.FindByEmailAsync(email));
        }

        [HttpGet]
        public async Task<ActionResult<IdentityUser>> FindByName(string name)
        {
            return this.Ok(await this.userManager.FindByNameAsync(name));
        }

        [HttpPut]
        public async Task<ActionResult<IdentityUser>> UpdateEmail(string email, string newEmail)
        {
            var requiredUser = await this.userManager.FindByEmailAsync(email);

            if(requiredUser == null)
            {
                return this.BadRequest("The user wasn't found");
            }

            requiredUser.EmailAddress = newEmail;
            var result = await this.userManager.UpdateAsync(requiredUser);
            await this.userManager.UpdateSecurityStampAsync(requiredUser);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<IdentityUser>> AddUser(string name, string email, string phone)
        {
            // This part of code can be deleted because UserManager takes over validation process

            /*var user = await this.userManager.FindByNameAsync(name);

            if(user != null)
            {
                return this.BadRequest();
            }*/

            var creationResult = await this.userManager.CreateAsync(new IdentityUser
            {
                UserName = name,
                EmailAddress = email,
                PhoneNumber = phone
            });

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Created("~api/User/AddUser", name);
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
        public async Task<ActionResult<IdentityUser>> UpdateUser(string name, string newUserName)
        {
            var user = await this.userManager.FindByNameAsync(name);

            if (user == null)
            {
                return this.BadRequest();
            }

            user.UserName = newUserName;
            var creationResult = await this.userManager.UpdateAsync(user);

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Ok(name);
        }

    }
}