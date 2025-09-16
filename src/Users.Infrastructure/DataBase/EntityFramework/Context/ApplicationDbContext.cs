using Core.PosTech8Nett.Api.Infra.DataBase.EntityFramework.EntityConfig.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Users.Domain.Entities.Identity;
using Users.Infrastructure.DataBase.EntityFramework.EntityConfig;

namespace Users.Infrastructure.DataBase.EntityFramework.Context
{
    public class ApplicationDbContext : IdentityDbContext<
      UsersEntitie,
      Roles,
      Guid,
      UserClaims,
      UserRoles,
      UserLogins,
      Claims,
      UserToken>
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options.  
        /// </summary>  
        /// <param name="options">The options to configure the database context.</param>  
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());

            builder.ApplyConfiguration(new AddressConfiguration());
            builder.ApplyConfiguration(new ContactConfiguration());
        }
    }
}

