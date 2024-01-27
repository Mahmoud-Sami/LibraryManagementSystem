using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Interfaces;
using MediatR;
using System.Transactions;

namespace LMS.Business.Commands.BorrowBooks
{
    public class BorrowBooksCommandHandler : IRequestHandler<BorrowBooksCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowRepositoy _borrowRepositoy;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public BorrowBooksCommandHandler(IBookRepository bookRepository, IBorrowRepositoy borrowRepositoy)
        {
            _bookRepository = bookRepository;
            _borrowRepositoy = borrowRepositoy;
        }

        public async Task<Result> Handle(BorrowBooksCommand request, CancellationToken cancellationToken)
        {
            if (request.BooksISBN is null || request.BooksISBN.Length == 0)
                return Result.Error("Select books to borrow");

            await _semaphore.WaitAsync();
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (!await _bookRepository.IsAllBooksAvailableAsync(request.BooksISBN))
                        return Result.Error("Some of the selected books are checked out already !, refresh the page and try again");

                    await _bookRepository.UpdateBooksAvailabilityStatusAsync(false, request.BooksISBN);
                    await _borrowRepositoy.BorrowAsync(request.userId, request.BooksISBN);

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return Result.Error(ex.Message);
                }
                finally 
                { 
                    _semaphore.Release();
                }
            }

            return Result.Ok();

        }
    }
}
