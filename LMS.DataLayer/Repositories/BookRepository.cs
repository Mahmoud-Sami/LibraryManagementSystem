using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Helpers;
using LMS.DataAccess.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace LMS.DataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly string _connectionString;
        private readonly DatabaseManager _databaseManager;

        public BookRepository(string connectionString, DatabaseManager databaseManager)
        {
            _connectionString = connectionString;
            _databaseManager = databaseManager;
        }

        public async Task<List<Book>> GetAllAsync()
        {
            List<Book> books = new List<Book>();

            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM books;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string ISBN = reader.GetString(reader.GetOrdinal("ISBN"));
                            string Title = reader.GetString(reader.GetOrdinal("Title"));
                            string Author = reader.GetString(reader.GetOrdinal("Author"));
                            bool IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"));

                            Book book = new()
                            {
                                ISBN = ISBN,
                                Title = Title,
                                Author = Author,
                                IsAvailable = IsAvailable,
                            };
                       

                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }
        public async Task<List<Book>> GetAllAsync(string searchText)
        {
            List<Book> books = new List<Book>();

            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM books WHERE ISBN LIKE @text OR Title LIKE @text OR Author LIKE @text;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@text", "%" + searchText + "%");
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string ISBN = reader.GetString(reader.GetOrdinal("ISBN"));
                            string Title = reader.GetString(reader.GetOrdinal("Title"));
                            string Author = reader.GetString(reader.GetOrdinal("Author"));
                            bool IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"));

                            Book book = new()
                            {
                                ISBN = ISBN,
                                Title = Title,
                                Author = Author,
                                IsAvailable = IsAvailable,
                            };


                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }


        public async Task InsertAsync(Book book)
        {
            string query = @"INSERT INTO Books (ISBN, Title, Author, IsAvailable) VALUES (@ISBN, @Title, @Author, @IsAvailable);";
            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@ISBN", book.ISBN),
                new MySqlParameter("@Title", book.Title),
                new MySqlParameter("@Author", book.Author),
                new MySqlParameter("@IsAvailable", book.IsAvailable),
            };

            await _databaseManager.ExecuteQueryAsync(query, parameters);
        }

        public async Task<bool> IsExistsAsync(string ISBN)
        {
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EXISTS(SELECT * FROM books WHERE ISBN = @ISBN) AS IsExists;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ISBN", ISBN);

                    bool isExists = Convert.ToBoolean(await command.ExecuteScalarAsync());
                    return isExists;
                }
            }
        }

        public async Task<bool> IsAllBooksAvailableAsync(params string[] BooksISBN)
        {
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT EXISTS(SELECT * FROM books WHERE ISBN IN ({ISBN}) AND IsAvailable = false) AS CheckedOut;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.AddArrayParameters("ISBN", BooksISBN);
                    bool CheckedOut = Convert.ToBoolean(await command.ExecuteScalarAsync());
                    return !CheckedOut;
                }
            }
        }

        public async Task<List<Borrow>> GetBorrowsAsync(int userId)
        {
            List<Borrow> borrows = new List<Borrow>();
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT ISBN, Title, Author, IsAvailable, Timestamp FROM library_db.borrowings JOIN books\r\nON bookISBN = books.ISBN\r\nWHERE userId = @userId;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            string ISBN = reader.GetString(reader.GetOrdinal("ISBN"));
                            string Title = reader.GetString(reader.GetOrdinal("Title"));
                            string Author = reader.GetString(reader.GetOrdinal("Author"));
                            bool IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable"));
                            DateTime Timestamp = reader.GetDateTime(reader.GetOrdinal("Timestamp"));

                            Borrow borrow = new Borrow()
                            {
                                Book = new Book()
                                {
                                    ISBN = ISBN,
                                    Title = Title,
                                    Author = Author,
                                    IsAvailable = IsAvailable,
                                },

                                Timestamp = Timestamp
                            };

                            borrows.Add(borrow);
                        }
                    }
                }
            }
            return borrows;
        }

        public async Task UpdateBooksAvailabilityStatusAsync(bool isAvailable, params string[] BooksISBN)
        {
            string query = @"UPDATE books SET IsAvailable = @IsAvailable WHERE `ISBN` IN ({ISBN})";
            using (MySqlConnection connection = new(_connectionString))
            {
                await connection.OpenAsync();
                using (MySqlCommand cmd = new(query, connection))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.AddArrayParameters("ISBN", BooksISBN);
                    cmd.Parameters.AddWithValue("@IsAvailable", isAvailable);

                    await cmd.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
        }

        
    }
}
