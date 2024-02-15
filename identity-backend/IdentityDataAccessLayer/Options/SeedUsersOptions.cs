﻿namespace IdentityDataAccessLayer.Options
{
    public sealed class SeedUsersOptions 
    {
        public string UserName { get; set; }

        public string NormalizedUserName { get; set; }

        public string EmailAddress { get; set; }

        public string NormalizedEmailAddress { get; set; }

        public bool IsEmailAddressConfirmed { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsPhoneNumberConfirmed { get; set; }
    }
}