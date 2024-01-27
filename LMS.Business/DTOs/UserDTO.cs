using LMS.DataAccess.Models;

namespace LMS.Business.DTOs
{
    public class UserDTO
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Username { get; init; }
        public List<BorrowDTO> Borrows { get; } = new List<BorrowDTO>();

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Username = user.Username;
            foreach (Borrow b in user.Borrows)
            {
                Borrows.Add(new BorrowDTO(b)
                {
                    Book = new BookDTO(b.Book),
                    Timestamp = b.Timestamp,
                });
            }
        }
    }
}
