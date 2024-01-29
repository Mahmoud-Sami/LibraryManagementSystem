using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Interfaces;
using MediatR;
using System.Transactions;

namespace LMS.Business.Commands.ReturnBooks
{
    public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepositoy _borrowRepositoy;

        public ReturnBookCommandHandler(IBookRepository bookRepository, IBorrowRepositoy borrowRepositoy)
        {
            _bookRepository = bookRepository;
            _borrowRepositoy = borrowRepositoy;
        }

        public async Task<Result> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            if (request.BooksISBN is null || request.BooksISBN.Length == 0)
                return Result.Error("Select books to return");

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    await _bookRepository.UpdateBooksAvailabilityStatusAsync(true, request.BooksISBN);
                    await _borrowRepositoy.ReturnAsync(request.userId, request.BooksISBN);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return Result.Error(ex.Message);
                }
            }

            return Result.Ok();
        }
    }
}
