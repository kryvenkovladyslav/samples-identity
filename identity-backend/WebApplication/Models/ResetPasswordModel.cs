using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public sealed class ResetPasswordModel
    {
        [Required]
        public string UserName { get; init; }

        [Required]
        public string Token { get; init; }

        [Required]
        public string NewPassword { get; init; }
    }
}