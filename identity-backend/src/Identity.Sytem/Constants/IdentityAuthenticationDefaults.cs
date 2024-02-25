namespace Identity.System.Constants
{
    public static class IdentityAuthenticationDefaults
    {
        public static string CookiePrefix { get; private set; } = "Identity";

        public static string ApplicationScheme { get; private set; } = CookiePrefix + ".Application";

        public static string ExternalScheme { get; private set; } = CookiePrefix + ".External";

        public static string TwoFactorRememberMeScheme { get; private set; } = CookiePrefix + ".TwoFactorRememberMe";

        public static string TwoFactorUserIdScheme { get; private set; } = CookiePrefix + ".TwoFactorUserId";
    }
}