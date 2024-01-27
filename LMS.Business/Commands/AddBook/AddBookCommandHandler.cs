using LMS.DataAccess.Abstractions;
using LMS.DataAccess.Models;
using MediatR;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;

namespace LMS.Business.Commands.AddBook
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Result>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            Result requestValidation = ValidateRequest(request);
            if (requestValidation.Failed)
                return requestValidation;

            if (await _bookRepository.IsExistsAsync(request.ISBN))
                return Result.Error($"Book with ISBN {request.ISBN} already exists");

            var book = new Book()
            {
                ISBN = request.ISBN.Trim(), 
                Title = request.Title.Trim(), 
                Author = request.Author.Trim(),
                IsAvailable = true
            };

            try
            {
                await _bookRepository.InsertAsync(book);
            }
            catch (MySqlException ex)
            {
                return Result.Error(ex.Message);
            }

            return Result.Ok($"Book with ISBN {book.ISBN} inserted successfully");
        }

        private Result ValidateRequest(AddBookCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.ISBN))
                return Result.Error("ISBN field is missing");

            if (string.IsNullOrWhiteSpace(request.Title))
                return Result.Error("Title field is missing");

            if (string.IsNullOrWhiteSpace(request.Author))
                return Result.Error("Author field is missing");

            if (!IsStandardISBN(request.ISBN))
                return Result.Error("ISBN should be consist of 10 or 13 digit");

            return Result.Ok();
        }

        private bool IsStandardISBN(string ISBN) => Regex.IsMatch(ISBN, @"^\d{10}(?:\d{3})?$");
     
    }
}
