namespace IdentitySystem.Constants
{
    public static class IdentityAuthenticationDefaults
    {
        private static readonly string CookiePrefix = "Identity";

        public static readonly string ApplicationScheme = CookiePrefix + ".Application";

        public static readonly string ExternalScheme = CookiePrefix + ".External";

        public static readonly string TwoFactorRememberMeScheme = CookiePrefix + ".TwoFactorRememberMe";

        public static readonly string TwoFactorUserIdScheme = CookiePrefix + ".TwoFactorUserId";
    }
}