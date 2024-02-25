using IdentitySystem.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebApplication.Models;
using IdentityUser = IdentityDataAccessLayer.Models.IdentityUser;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class PasswordController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;

        private readonly ISMSConfirmationService<IdentityUser, Guid> confirmationService;

        public PasswordController(UserManager<IdentityUser> userManager, ISMSConfirmationService<IdentityUser, Guid> confirmationService)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.confirmationService = confirmationService ?? throw new ArgumentNullException(nameof(confirmationService));
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            if(user == null)
            {
                return this.BadRequest("The user with specified name was not found");
            }

            var result = await this.userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Succeeded);
        }

        [HttpGet]
        public async Task<ActionResult> GeneratePasswordChangeToken(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return this.BadRequest("The name of a user was not specified");
            }

            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return this.BadRequest("The user with specified name was not found");
            }

            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);

            this.confirmationService.SendMessage(user, token);

            return this.Ok(token);
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                return this.BadRequest("The user with specified name was not found");
            }

            var result = await this.userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Succeeded);
        }
    }
}