using System.Data;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace LMS.DataAccess.Helpers
{
    public class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ExecuteQueryAsync(string query, MySqlParameter[] parameters)
        {
            using (MySqlConnection connection = new(_connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    
                    await cmd.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }
    }
}
