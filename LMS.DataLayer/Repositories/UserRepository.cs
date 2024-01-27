using LMS.DataAccess.Helpers;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Models;
using MySql.Data.MySqlClient;

namespace LMS.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly DatabaseManager _databaseManager;

        public UserRepository(string connectionString, DatabaseManager databaseManager)
        {
            _connectionString = connectionString;
            _databaseManager = databaseManager;
        }

        public async Task<bool> IsExistsAsync(string username)
        {
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EXISTS(SELECT * FROM users WHERE username = @username) AS IsExists;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);

                    bool isExists = Convert.ToBoolean(await command.ExecuteScalarAsync());
                    return isExists;
                }
            }
        }
        public async Task<User?> GetUserAsync(string username, string password)
        {
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT users.id, users.name, users.username from users WHERE `username` = @username AND `password` = @password;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                            return null;

                        reader.Read();

                        int id = reader.GetInt32(reader.GetOrdinal("id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string _username = reader.GetString(reader.GetOrdinal("username"));

                        User user = new User()
                        {
                            Id = id,
                            Name = name,
                            Username = _username
                        };

                        return user;
                    }
                }
            }
        }

        public async Task RegisterAsync(User user)
        {
            string query = @"INSERT INTO Users (name, username, password) VALUES (@name, @username, @password);";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@name", user.Name),
                new MySqlParameter("@username", user.Username),
                new MySqlParameter("@password", user.Password)
            };

            await _databaseManager.ExecuteQueryAsync(query, parameters);
        }
    }
}
