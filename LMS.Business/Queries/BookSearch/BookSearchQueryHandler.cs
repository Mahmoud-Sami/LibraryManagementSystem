using LMS.Business.DTOs;
using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Models;
using MediatR;

namespace LMS.Business.Queries.BookSearch
{
    public class BookSearchQueryHandler : IRequestHandler<BookSearchQuery, Result<List<BookDTO>>>
    {
        private readonly IBookRepository _bookRepository;

        public BookSearchQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<List<BookDTO>>> Handle(BookSearchQuery request, CancellationToken cancellationToken)
        {
            List<Book> books = await _bookRepository.GetAllAsync(request.text.Trim());
            List<BookDTO> bookDTOs = books.Select(book => new BookDTO(book)).ToList();
            return Result.Ok(bookDTOs, "Success");
        }
    }
}
