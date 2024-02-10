using IdentitySystem.Abstract;
using System;

namespace IdentitySystem.Models
{
    public abstract class BaseApplicationUser : IApplicationUser<string>
    {
        public string ID { get; set; }

        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string EmailAddress { get; set; }

        public string NormalizedEmailAddress { get; set; }

        public bool IsEmailAddressConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }

        public BaseApplicationUser() { }

        public BaseApplicationUser(string userName) 
        {
            this.ID = Guid.NewGuid().ToString();
            this.UserName = userName;
            this.UserName = userName.ToUpper();
        }
    }
}