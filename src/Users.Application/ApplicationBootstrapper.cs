using Microsoft.Extensions.DependencyInjection;
using Users.Application.Services;
using Users.Application.Services.Interfaces;

namespace Users.Application
{
    public static class ApplicationBootstrapper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IAuthenticationServices, AuthenticationServices>();
            services.AddTransient<IUserServices, UserServices>();
        }
    }
}
