using LMS.DataAccess.Models;

namespace LMS.Business.DTOs
{
    public class BorrowDTO
    {
        public BorrowDTO(Borrow borrow)
        {
            Book = new BookDTO(borrow.Book);
            Timestamp = borrow.Timestamp;
        }
        public BookDTO Book { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
