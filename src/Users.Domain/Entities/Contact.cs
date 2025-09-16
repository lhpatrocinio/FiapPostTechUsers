using System;
using Users.Domain.Entities.Identity;
using Users.Domain.Enum;

namespace Users.Domain.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public ContactTypeEnum IdContactType { get; set; }
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public UsersEntitie User { get; set; }
    }
}
