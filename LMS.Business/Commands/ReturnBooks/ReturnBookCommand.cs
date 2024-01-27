using MediatR;

namespace LMS.Business.Commands.ReturnBooks
{
    public record ReturnBookCommand(int userId, params string[] BooksISBN) : IRequest<Result>
    {
    }
}
