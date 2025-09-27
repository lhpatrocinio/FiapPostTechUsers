using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Events;

namespace Users.Application.Events
{
    public interface IUserCreatedEventHandler
    {
        void PublishUserCreatedEvent(UserCreatedEvent user);
    }
}
