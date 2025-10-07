using Microsoft.Extensions.DependencyInjection;
using Users.Application.Events;
using Users.Application.Repository;
using Users.Infrastructure.DataBase.Repository;
using Users.Infrastructure.Events;

namespace Users.Infrastructure
{
    public static class InfraBootstrapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();

            services.AddTransient<IUserCreatedEventHandler, UserCreatedEventHandler>();
            services.AddTransient<IUserEventRepository, UserEventRepository>();
        }
    }
}
