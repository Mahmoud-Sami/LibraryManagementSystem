using LMS.Business;
using LMS.Business.Commands.BorrowBooks;
using LMS.Business.DTOs;
using LMS.Business.Queries.BookSearch;
using LMS.Business.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace LMS.UI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IMediator _mediator;

        public string TextSearch { get; set; }
        public List<BookDTO> Books { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        public async Task<IActionResult> OnGet()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToPage("/account/login");

            string searchText = Request.Query["text"];
            if (string.IsNullOrWhiteSpace(searchText))
            {
                Result<List<BookDTO>> books = await _mediator.Send(new GetBooksQuery());
                Books = books.Data;
            }
            else
            {
                TextSearch = searchText;

                Result<List<BookDTO>> books = await _mediator.Send(new BookSearchQuery(searchText));
                Books = books.Data;
            }
            

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string[] selectedISBNs = Request.Form["SelectedRows"].Skip(1).ToArray();
            Result result = await _mediator.Send(new BorrowBooksCommand(int.Parse(userId), selectedISBNs));
            if (result.Failed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }

            return RedirectToPage();
        }
    }
}