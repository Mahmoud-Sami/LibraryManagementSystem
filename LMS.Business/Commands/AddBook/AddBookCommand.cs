using MediatR;

namespace LMS.Business.Commands.AddBook
{
    public record AddBookCommand (string ISBN, string Title, string Author) : IRequest<Result>
    {
    }
}
