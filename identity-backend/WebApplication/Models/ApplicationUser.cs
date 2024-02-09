using IdentitySystem.Models;

namespace WebApplication.Models
{
    public sealed class ApplicationUser : BaseApplicationUser
    {
        public ApplicationUser() { }
        public ApplicationUser(string userName) : base(userName)
        {
        }
    }
}