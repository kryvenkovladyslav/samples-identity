namespace WebApplication.Infrastructure.Options
{
    public sealed class DefaultEmailValidationOptions
    {
        public static string Position { get; private set; } = nameof(DefaultEmailValidationOptions);

        public string[] AllowedDomains { get; set; }
    }
}