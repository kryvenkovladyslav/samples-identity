using IdentitySystem.Abstract;
using IdentitySystem.Models;
using Microsoft.Extensions.Logging;
using System;

namespace IdentitySystem.ConfirmationServices
{
    public class SMSConfirmationService<TUser, TKey> : ISMSConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        private readonly ILogger<SMSConfirmationService<TUser, TKey>> logger;

        public SMSConfirmationService(ILogger<SMSConfirmationService<TUser, TKey>> logger)
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