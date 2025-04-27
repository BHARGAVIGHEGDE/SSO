using ssobackend.Models;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace ssobackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            User user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        HashPassword = reader.GetString(2)
                    };
                }
            }

            return user;
        }
          public async Task<UserToken> GetTokenByUserIdAsync(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand cmd = new SqlCommand("SELECT * FROM UserTokens WHERE UserId = @UserId ", connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new UserToken
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                Token = reader.GetString(reader.GetOrdinal("Token")),
                                ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate"))
                            };
                        }
                        else
                        {
                            return null; 
                        }
                    }
                }
            }
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            User user = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT * FROM Users WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);
                var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        HashPassword = reader.GetString(2)
                    };
                }
            }

            return user;
        }

        public async Task StoreTokenInDb(int userId, string token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var expirationDate = DateTime.Now.AddHours(1);

                var command = new SqlCommand("INSERT INTO UserTokens (UserId, Token, ExpirationDate) VALUES (@UserId, @Token, @ExpirationDate)", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Token", token);
                command.Parameters.AddWithValue("@ExpirationDate", expirationDate);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
