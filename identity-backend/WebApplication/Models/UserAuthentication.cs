using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public sealed class UserAuthentication 
    {
        [Required]
        public string UserName { get; init; }

        [Required]
        public string Email { get; init; }

        [Required]
        public string PhoneNumber { get; init; }

        [Required]
        public string Password { get; init; }
    }
}