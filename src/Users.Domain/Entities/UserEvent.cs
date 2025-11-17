using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities.Identity;

namespace Users.Domain.Entities
{
    public class UserEvent
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreateAt { get; set; }
        public EventUser Event { get; set; }
        public string email { get; set; }
    }

    public enum EventUser
    {
        create = 1,
        block=2,
        active = 3,
        delete = 4
    }
}
