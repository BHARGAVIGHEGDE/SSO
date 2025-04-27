using ssobackend.Models;
using System.Threading.Tasks;

namespace ssobackend.Repositories
{
    public interface IUserRepository
    {

        Task<User> GetUserByIdAsync(int id);
        Task<UserToken> GetTokenByUserIdAsync(int userId);
        Task<User> GetUserByUsernameAsync(string username);
        Task StoreTokenInDb(int userId, string token);
    }
}
