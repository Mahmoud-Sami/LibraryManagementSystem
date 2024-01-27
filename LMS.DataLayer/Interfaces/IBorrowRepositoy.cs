namespace LMS.DataAccess.Interfaces
{
    public interface IBorrowRepositoy
    {
        Task BorrowAsync(int userId, params string[] booksISBN);
        Task ReturnAsync(int userId, params string[] booksISBN);

    }
}
