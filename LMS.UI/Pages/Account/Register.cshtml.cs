using LMS.Business;
using LMS.Business.Commands.Account.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LMS.UI.Pages.Account
{
    public class RegistrationInputModel
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

    }
    public class RegisterModel : PageModel
    {
        private readonly IMediator _mediator;

        public RegisterModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public RegistrationInputModel RegistrationInput { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            Result result = await _mediator.Send(new RegisterCommand(RegistrationInput.Name, RegistrationInput.Username, RegistrationInput.Password, RegistrationInput.ConfirmPassword));
            if (result.Failed)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }
            return RedirectToPage("/index");
        }
    }
}
