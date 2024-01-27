using MediatR;

namespace LMS.Business.Commands.BorrowBooks
{
    public record BorrowBooksCommand(int userId, params string[] BooksISBN) : IRequest<Result>
    {
    }
}
