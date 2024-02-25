using System;
using Identity.Abstract.Models;
using Microsoft.Extensions.Logging;
using IdentitySystem.Abstract.Interfaces;

namespace Identity.System.ConfirmationServices
{
    public class PhoneNumberConfirmationService<TUser, TKey> : IPhoneNumberConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : IdentitySystemUser<TKey>
    {
        private readonly ILogger<PhoneNumberConfirmationService<TUser, TKey>> logger;

        public PhoneNumberConfirmationService(ILogger<PhoneNumberConfirmationService<TUser, TKey>> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual void SendMessage(TUser user, params string[] body)
        {
            this.logger.LogInformation($"Sending a message to: {user.PhoneNumber}");

            foreach (var item in body)
            {
                this.logger.LogInformation(item);
            }

            this.logger.LogInformation("The message has been sent");
        }
    }
}