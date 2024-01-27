using LMS.Business.DTOs;
using MediatR;

namespace LMS.Business.Queries.GetBorrowedBooks
{
    public record BorrowedBookQuery(int userId) : IRequest<Result<List<BorrowDTO>>>
    {
    }
}
