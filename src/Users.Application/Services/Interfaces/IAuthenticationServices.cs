using Users.Application.Dtos.Requests;

namespace Users.Application.Services.Interfaces
{
    public interface IAuthenticationServices
    {
        Task<string> LoginAsync(LoginRequest request);
    }
}
