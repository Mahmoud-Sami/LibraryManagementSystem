using LMS.Business.DTOs;
using MediatR;

namespace LMS.Business.Queries.GetAll
{
    public record GetBooksQuery : IRequest<Result<List<BookDTO>>>
    {
    }
}
