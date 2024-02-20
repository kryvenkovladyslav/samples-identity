using IdentitySystem.Abstract;
using IdentitySystem.Models;
using Microsoft.Extensions.Logging;
using System;

namespace IdentitySystem.ConfirmationServices
{
    public class EmailConfirmationService<TUser, TKey> : IEmailConfirmationService<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : BaseApplicationUser<TKey>
    {
        private readonly ILogger<EmailConfirmationService<TUser, TKey>> logger;

        public EmailConfirmationService(ILogger<EmailConfirmationService<TUser, TKey>> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public virtual void SendMessage(TUser user, string subject, params string[] body)
        {
            this.logger.LogInformation($"Sending a message to: {user.EmailAddress}");
            this.logger.LogInformation($"Subject is: {subject}");

            foreach (var item in body)
            {
                this.logger.LogInformation(item);
            }

            this.logger.LogInformation("The message has been sent");
        }
    }
}