using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public sealed class ChangePasswordModel
    {
        [Required]
        public string UserName { get; init; }

        [Required]
        public string NewPassword { get; init; }

        [Required]
        public string CurrentPassword { get; init; }
    }
}