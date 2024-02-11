using IdentitySystem.Abstract;
using System;

namespace IdentitySystem.Models
{
    public abstract class BaseApplicationUser<TKey> : IApplicationUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual TKey ID { get; set; }

        public virtual string UserName { get; set; }

        public virtual string NormalizedUserName { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string NormalizedEmailAddress { get; set; }

        public virtual bool IsEmailAddressConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool IsPhoneNumberConfirmed { get; set; }

        public BaseApplicationUser() { }

        public BaseApplicationUser(string userName)
        {
            this.UserName = userName;
        }
    }

    public abstract class BaseApplicationUser : IApplicationUser<string>
    {
        public virtual string ID { get; set; }

        public virtual string UserName { get; set; }

        public virtual string NormalizedUserName { get; set; }

        public virtual string EmailAddress { get; set; }

        public virtual string NormalizedEmailAddress { get; set; }

        public virtual bool IsEmailAddressConfirmed { get; set; }

        public virtual string PhoneNumber { get; set; }

        public virtual bool IsPhoneNumberConfirmed { get; set; }

        public BaseApplicationUser() { }

        public BaseApplicationUser(string userName) 
        {
            this.ID = Guid.NewGuid().ToString();
            this.UserName = userName;
            this.UserName = userName.ToUpper();
        }
    }
}