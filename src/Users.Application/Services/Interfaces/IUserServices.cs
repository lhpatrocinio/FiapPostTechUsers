using Users.Application.Dtos.Requests;
using Users.Application.Dtos.Response;

namespace Users.Application.Services.Interfaces
{
    public interface IUserServices
    {
        Task CreateAsync(CreateUserRequest request);
        Task<UserResponse> GetByEmailAsync(GetUserByEmailRequest request);
        Task<UserResponse> GetByNickNameAsync(GetUserByNickNameRequest request);
        Task<UserResponse> GetByIdAsync(GetUserByIdRequest request);
        Task DeleteAsync(DeleteUserRequest request);
        Task BlockUserAsync(BlockUserRequest request);

    }
}
