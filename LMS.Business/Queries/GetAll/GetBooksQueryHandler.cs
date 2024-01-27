using LMS.Business.DTOs;
using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Models;
using MediatR;

namespace LMS.Business.Queries.GetAll
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, Result<List<BookDTO>>>
    {
        private readonly IBookRepository _bookRepository;

        public GetBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<List<BookDTO>>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            List<Book> books = await _bookRepository.GetAllAsync();
            List<BookDTO> bookDTOs = books.Select(book => new BookDTO(book)).ToList();
            return Result.Ok(bookDTOs, "Success");
        }
    }
}
