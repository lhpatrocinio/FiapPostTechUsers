using Microsoft.Extensions.DependencyInjection;
using Users.Application.Repository;
using Users.Infrastructure.DataBase.Repository;

namespace Users.Infrastructure
{
    public static class InfraBootstrapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
