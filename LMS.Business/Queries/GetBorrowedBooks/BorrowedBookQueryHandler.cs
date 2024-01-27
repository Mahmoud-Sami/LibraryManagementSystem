using LMS.Business.DTOs;
using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Models;
using MediatR;

namespace LMS.Business.Queries.GetBorrowedBooks
{

    public class BorrowedBookQueryHandler : IRequestHandler<BorrowedBookQuery, Result<List<BorrowDTO>>>
    {
        private readonly IBookRepository _bookRepository;

        public BorrowedBookQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<List<BorrowDTO>>> Handle(BorrowedBookQuery request, CancellationToken cancellationToken)
        {
            List<Borrow> borrows = await _bookRepository.GetBorrowsAsync(request.userId);
            List<BorrowDTO> borrowDTOs = borrows.Select(b => new BorrowDTO(b)).ToList();

            return Result.Ok(borrowDTOs);
        }
    }
}
