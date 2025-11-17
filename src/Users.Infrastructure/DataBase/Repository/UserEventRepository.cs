using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Repository;
using Users.Domain.Entities;
using Users.Domain.Entities.Identity;
using Users.Infrastructure.DataBase.EntityFramework.Context;

namespace Users.Infrastructure.DataBase.Repository
{
    public class UserEventRepository: IUserEventRepository
    {
        private readonly ApplicationDbContext _context;

        public UserEventRepository( ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddEvent(UserEvent entitie)
        {
           await _context.AddAsync(entitie);
           await _context.SaveChangesAsync();
        }
    }
}
