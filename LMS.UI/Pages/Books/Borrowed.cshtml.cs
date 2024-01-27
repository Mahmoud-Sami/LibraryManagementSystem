using LMS.Business.Queries.GetAll;
using LMS.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using LMS.Business.DTOs;
using LMS.Business.Queries.GetBorrowedBooks;
using LMS.Business.Commands.BorrowBooks;
using System.Security.Claims;
using LMS.Business.Commands.ReturnBooks;

namespace LMS.UI.Pages.Books
{
    public class BorrowedModel : PageModel
    {
        private readonly IMediator _mediator;

        public List<BorrowDTO> Borrows { get; set; }
        public BorrowedModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnGet()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                return RedirectToPage("/account/login");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Result<List<BorrowDTO>> borrows = await _mediator.Send(new BorrowedBookQuery(int.Parse(userId)));
            Borrows = borrows.Data;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            string[] selectedISBNs = Request.Form["SelectedRows"].Skip(1).ToArray();
            Result result = await _mediator.Send(new ReturnBookCommand(int.Parse(userId), selectedISBNs));


            return RedirectToPage();
        }
    }
}
