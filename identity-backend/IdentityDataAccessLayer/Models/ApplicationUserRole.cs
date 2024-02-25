using Identity.Abstract.Models;
using System;

namespace IdentityDataAccessLayer.Models
{
    public class ApplicationUserRole : IdentitySystemUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }
}