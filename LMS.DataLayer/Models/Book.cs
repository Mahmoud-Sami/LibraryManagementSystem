namespace LMS.DataAccess.Models
{
    public class Book
    {
        public required string ISBN { get; init; }
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required bool IsAvailable { get; set; }
    }
}
