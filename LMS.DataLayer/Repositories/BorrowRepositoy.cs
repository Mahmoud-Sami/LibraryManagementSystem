using LMS.DataAccess.Helpers;
using LMS.DataAccess.Interfaces;
using MySql.Data.MySqlClient;
using System.Text;

namespace LMS.DataAccess.Repositories
{
    public class BorrowRepositoy : IBorrowRepositoy
    {
        private readonly string _connectionString;
        private readonly DatabaseManager _databaseManager;

        public BorrowRepositoy(string connectionString, DatabaseManager databaseManager)
        {
            _connectionString = connectionString;
            _databaseManager = databaseManager;
        }

        public async Task BorrowAsync(int userId, params string[] booksISBN)
        {
            // The number of paramters is (userId + booksISBN)
            List<MySqlParameter> parameters = new(booksISBN.Length + 1);
            parameters.Add(new MySqlParameter("@userId", userId));

            StringBuilder query = new StringBuilder(@"INSERT INTO `borrowings` (`userId`, `bookISBN`, `Timestamp`) VALUES ");
            for (int i = 0; i < booksISBN.Length; i++) 
            {
                query.Append($"(@userId, @ISBN_{i}, utc_time())" + (i < booksISBN.Length - 1 ? ',' : ';'));
                parameters.Add(new MySqlParameter($"@ISBN_{i}", booksISBN[i]));
            }

            await _databaseManager.ExecuteQueryAsync(query.ToString(), parameters.ToArray());
        }

        public async Task ReturnAsync(int userId, params string[] booksISBN)
        {
            // The number of paramters is (userId + booksISBN)
            List<MySqlParameter> parameters = new(booksISBN.Length + 1);
            parameters.Add(new MySqlParameter("@userId", userId));

            StringBuilder query = new StringBuilder(@"DELETE FROM `borrowings` WHERE `userId` = @userId AND `bookISBN` IN (");
            for (int i = 0; i < booksISBN.Length; i++)
            {
                query.Append($"@ISBN_{i}" + (i < booksISBN.Length - 1 ? ',' : ");"));
                parameters.Add(new MySqlParameter($"@ISBN_{i}", booksISBN[i]));
            }

            await _databaseManager.ExecuteQueryAsync(query.ToString(), parameters.ToArray());
        }
    }
}
