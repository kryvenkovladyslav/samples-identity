using IdentitySystem.Models;
using System;

namespace IdentityDataAccessLayer.Models
{
    public class IdentityUserRole : BaseApplicationUserRole<Guid>
    {
        public virtual IdentityUser User { get; set; }

        public virtual IdentityRole Role { get; set; }
    }
}