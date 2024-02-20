namespace IdentitySystem.Database.Configuration.User
{
    public static class ApplicationUserConfigurationDefaults
    {
        public static string TableName { get; private set; } = "IdentityUser";

        public static string UserName { get; private set; } = nameof(UserName);

        public static string NormalizedUserName { get; private set; } = nameof(NormalizedUserName);

        public static string EmailAddress { get; private set; } = nameof(EmailAddress);

        public static string NormalizedEmailAddress { get; private set; } = nameof(NormalizedEmailAddress);

        public static string IsEmailAddressConfirmed { get; private set; } = nameof(IsEmailAddressConfirmed);

        public static string PhoneNumber { get; private set; } = nameof(PhoneNumber);

        public static string IsPhoneNumberConfirmed { get; private set; } = nameof(IsPhoneNumberConfirmed);

        public static string SecurityStamp { get; private set; } = nameof(SecurityStamp);
    }
}