namespace WebApplication.Models
{
    public sealed class UserAuthentication 
    {
        public string UserName { get; init; }

        public string Email { get; init; }

        public string PhoneNumber { get; init; }

        public string Password { get; init; }
    }
}