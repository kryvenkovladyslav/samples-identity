namespace WebApplication.Infrastructure.Authentication
{
    public static class AuthenticationDefaults
    {
        public static string CustomScheme { get; private set; } = "default";

        public static string RestrictedScheme { get; private set; } = "restricted";
    }
}