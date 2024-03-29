﻿using IdentityDataAccessLayer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Models;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

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

        [HttpPut]
        public async Task<ActionResult<ApplicationUser>> UpdateEmail(string email, string newEmail)
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
        public async Task<ActionResult<ApplicationUser>> AddUser(string name, string email, string phone)
        {
            // This part of code can be deleted because UserManager takes over validation process

            /*var user = await this.userManager.FindByNameAsync(name);

            if(user != null)
            {
                return this.BadRequest();
            }*/

            var creationResult = await this.userManager.CreateAsync(new ApplicationUser
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

        [HttpPost]
        public async Task<ActionResult<ApplicationUser>> AddUserModel([FromBody] UserAuthentication model)
        {
            var creationResult = await this.userManager.CreateAsync(new ApplicationUser
            {
                UserName = model.UserName,
                Password = model.Password,
                EmailAddress = model.Email,
                PhoneNumber = model.PhoneNumber
            }, model.Password);

            if (!creationResult.Succeeded)
            {
                return this.BadRequest(creationResult.Errors);
            }

            return this.Created("~api/User/AddUser", model.UserName);
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