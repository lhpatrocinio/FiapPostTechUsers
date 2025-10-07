using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.Entities;

namespace Users.Application.Repository
{
    public interface IUserEventRepository
    {
        Task AddEvent(UserEvent entitie);
    }
}
