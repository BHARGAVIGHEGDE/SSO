using ssobackend.Models;
using System.Threading.Tasks;

namespace ssobackend.Services
{
  public interface IUserService
  {
    Task<User> GetUserByIdAsync(int id);
    Task<UserToken> GetTokenByUserIdAsync(int userId);
    Task<User> AuthenticateUserAsync(string username, string password);
    string GenerateJwtToken(User user);
    Task StoreTokenInDb(int userId, string token);
  }
}
