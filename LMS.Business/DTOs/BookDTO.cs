using LMS.DataAccess.Models;

namespace LMS.Business.DTOs
{
    public class BookDTO
    {
        public string ISBN { get; init; }
        public string Title { get; init; }
        public string Author { get; init; }
        public bool IsAvailable { get; init; }

        public BookDTO(string iSBN, string title, string author, bool isAvailable)
        {
            ISBN = iSBN;
            Title = title;
            Author = author;
            IsAvailable = isAvailable;
        }

        public BookDTO(Book book)
        {
            ISBN = book.ISBN;
            Title = book.Title;
            Author = book.Author;
            IsAvailable = book.IsAvailable;
        }
    }
}
