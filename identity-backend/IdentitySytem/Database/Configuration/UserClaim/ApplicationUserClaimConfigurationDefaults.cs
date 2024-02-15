namespace IdentitySystem.Database.Configuration.UserClaim
{
    public static class ApplicationUserClaimConfigurationDefaults
    {
        public static string TableName { get; private set; } = "IdentityUserClaim";

        public static string UserID { get; private set; } = nameof(UserID);

        public static string ClaimType { get; private set; } = nameof(ClaimType);

        public static string ClaimValue { get; private set; } = nameof(ClaimValue);
    }
}