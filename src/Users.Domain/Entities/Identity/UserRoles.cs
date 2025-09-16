using Microsoft.AspNetCore.Identity;
using System;

namespace Users.Domain.Entities.Identity
{
    public class UserRoles : IdentityUserRole<Guid>
    {
        public UsersEntitie User { get; set; }
        public Roles Role { get; set; }
    }
}
