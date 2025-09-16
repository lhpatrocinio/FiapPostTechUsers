using Users.Domain.Entities.Identity;

namespace Users.Application.Repository
{
    public interface IUserRepository
    {
        Task CreateAsync(UsersEntitie entity, string password);
        void Update(UsersEntitie entity);
        Task DeleteAsync(UsersEntitie entity);
        Task BlockUserAsync(UsersEntitie user, bool enableBlocking);
        Task<UsersEntitie> GetByIdAsync(object id);
        Task<UsersEntitie> GetByEmailAsync(string email);
        Task<UsersEntitie> GetByNicknameAsync(string nickname);
        Task<bool> CheckPasswordAsync(UsersEntitie user, string password);
        Task<IList<string>> GetRolesUser(UsersEntitie user);
        Task AccessFailedAsync(UsersEntitie user);
        Task<bool> CheckLockedOutAsync(UsersEntitie user);
    }
}
