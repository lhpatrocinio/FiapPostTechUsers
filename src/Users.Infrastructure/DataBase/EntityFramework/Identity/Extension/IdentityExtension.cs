using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Users.Domain.Entities.Identity;
using Users.Infrastructure.DataBase.EntityFramework.Context;

namespace Users.Infrastructure.DataBase.EntityFramework.Identity.Extension
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityExtension(this IServiceCollection services)
        {
            services.AddIdentity<UsersEntitie, Roles>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                //Password
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                //User
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.SignIn.RequireConfirmedEmail = true;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
            });

            return services;
        }
    }
}
