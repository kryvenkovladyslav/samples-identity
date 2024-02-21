﻿using IdentitySystem.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using IdentityUser = IdentityDataAccessLayer.Models.IdentityUser;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class ConfirmationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ISMSConfirmationService<IdentityUser, Guid> phoneConfirmationService;
        private readonly IEmailConfirmationService<IdentityUser, Guid> emailConfirmationService;

        public ConfirmationController(UserManager<IdentityUser> userManager, 
            ISMSConfirmationService<IdentityUser, Guid> phoneConfirmationService,
            IEmailConfirmationService<IdentityUser, Guid> emailConfirmationService)
        {
            this.userManager = userManager;
            this.phoneConfirmationService = phoneConfirmationService;
            this.emailConfirmationService = emailConfirmationService;
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmEmail(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if(user == null)
            {
                this.BadRequest("The user with specified name was not found");
            }

            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

            var url = $"http://localhost:5000/Confirmation/ValidateEmailConfirmation/{user.ID}/{token}";

            this.emailConfirmationService.SendMessage(user, "Email Confirmation", "Confirm",
                $"Please Click here to confirm you email: {url}");

            return this.Ok(url);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateEmailConfirmation(string id, string token)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.BadRequest("The user with specified name was not found");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, token);

            if(!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Succeeded);
        }

        [HttpGet]
        public async Task<ActionResult> ConfirmPhoneNumber(string userName)
        {
            var user = await this.userManager.FindByNameAsync(userName);

            if (user == null)
            {
                this.BadRequest("The user with specified name was not found");
            }

            var token = await this.userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

            var url = $"http://localhost:5000/Confirmation/ValidateEmailConfirmation/{user.ID}/{token}";

            this.emailConfirmationService.SendMessage(user, "Email Confirmation", "Confirm",
                $"Please Click here to confirm you email: {url}");

            return this.Ok(url);
        }

        [HttpPost]
        public async Task<ActionResult> ValidatePhoneNumberConfirmation(string id, string token)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.BadRequest("The user with specified name was not found");
            }

            var test = await this.userManager.VerifyChangePhoneNumberTokenAsync(user, token, user.PhoneNumber);
            var result = await this.userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, token);

            return this.Ok(result);
        }
    }
}