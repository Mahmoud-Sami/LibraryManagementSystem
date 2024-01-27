namespace LMS.DataAccess.Models
{
    public class Borrow
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
