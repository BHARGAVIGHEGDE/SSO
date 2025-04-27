using ssobackend.Models;
using ssobackend.Repositories;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using System;

namespace ssobackend.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _secretKey;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _secretKey = configuration["Jwt:SecretKey"];
        }
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
        public async Task<UserToken> GetTokenByUserIdAsync(int userId)
        {
            return await _userRepository.GetTokenByUserIdAsync(userId);
        }
        public async Task<User> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null || !VerifyPassword(password, user.HashPassword))
            {
                return null;
            }
            return user;
        }
        public async Task StoreTokenInDb(int userId, string token)
        {
            await _userRepository.StoreTokenInDb(userId, token);
        }
        public string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "SSOApp",
                audience: "SSOApp",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool VerifyPassword(string enteredPassword, string storedHashPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHashPassword);
            }
            catch (BCrypt.Net.SaltParseException)
            {

                return false;
            }
        }
    }
}
