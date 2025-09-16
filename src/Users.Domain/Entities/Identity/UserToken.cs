using Microsoft.AspNetCore.Identity;
using System;

namespace Users.Domain.Entities.Identity
{
    public class UserToken : IdentityUserToken<Guid>
    {
    }
}
