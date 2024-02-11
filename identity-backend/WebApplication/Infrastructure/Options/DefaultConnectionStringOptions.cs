namespace WebApplication.Infrastructure.Options
{
    public sealed class DefaultConnectionStringOptions
    {
        public static string Position { get; } = nameof(DefaultConnectionStringOptions);

        public string IdentityDatabase { get; init; }
    }
}