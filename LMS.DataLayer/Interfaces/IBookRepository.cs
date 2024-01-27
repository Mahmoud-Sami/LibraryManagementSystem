using LMS.DataAccess.Models;

namespace LMS.DataAccess.Abstractions
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetAllAsync(string searchText);

        Task<List<Borrow>> GetBorrowsAsync(int userId);
        Task InsertAsync(Book book);
        Task<bool> IsExistsAsync(string ISBN);
        Task<bool> IsAllBooksAvailableAsync(params string[] BooksISBN);
        Task UpdateBooksAvailabilityStatusAsync(bool isAvailable, params string[] BooksISBN);
    }
}
