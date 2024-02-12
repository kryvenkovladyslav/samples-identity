using IdentitySystem.Models;
using System;

namespace IdentityDataAccessLayer.Models
{
    public class IdentityUserClaim : BaseApplicationUserClaim<Guid>
    {
        public virtual IdentityUser User { get; set; }
    }
}