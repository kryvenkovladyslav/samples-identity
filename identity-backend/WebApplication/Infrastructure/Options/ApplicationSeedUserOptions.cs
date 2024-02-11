namespace WebApplication.Infrastructure.Options
{
    public sealed class ApplicationSeedUserOptions
    {
        public static string Position { get; } = nameof(ApplicationSeedUserOptions);

        public string UserName { get; init; }

        public string NormalizedUserName { get; init; }

        public string EmailAddress { get; init; }

        public string NormalizedEmailAddress { get; init; }

        public bool IsEmailAddressConfirmed { get; init; }

        public string PhoneNumber { get; init; }

        public bool IsPhoneNumberConfirmed { get; init; }
    }
}