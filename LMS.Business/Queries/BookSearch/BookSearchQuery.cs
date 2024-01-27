using LMS.Business.DTOs;
using MediatR;

namespace LMS.Business.Queries.BookSearch
{
    public record BookSearchQuery(string text) : IRequest<Result<List<BookDTO>>>
    {
    }
}
