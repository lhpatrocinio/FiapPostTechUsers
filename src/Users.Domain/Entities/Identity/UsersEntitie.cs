using Microsoft.AspNetCore.Identity;

namespace Users.Domain.Entities.Identity
{
    public class UsersEntitie : IdentityUser<Guid>
    {
        public UsersEntitie()
        {
            Id = Guid.NewGuid();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string NickName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public Address Address { get; set; }
        public Contact Contact { get; set; }
        public List<UserRoles> UserRoles { get; set; }
    }
}
