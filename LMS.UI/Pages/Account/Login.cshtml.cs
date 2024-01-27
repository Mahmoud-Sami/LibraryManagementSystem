using LMS.Business.Commands.Account.Login;
using LMS.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using LMS.Business.DTOs;

namespace LMS.UI.Pages.Account
{
    public class LoginInputModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
    public class LoginModel : PageModel
    {
        private readonly IMediator _mediator;
        public LoginModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public bool LoginStatus { get; set; } = true;

        [BindProperty]
        public LoginInputModel LoginInput { get; set; }
        public void OnGet()
        {
            LoginStatus = true;
        }

        public async Task<IActionResult> OnPost()
        {
            Result<UserDTO> result = await _mediator.Send(new LoginCommand(LoginInput?.Username?.Trim(), LoginInput?.Password));

            if (result.Success)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.Data.Name),
                    new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // You can add additional properties here if needed
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties
                );

                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return Page();
            }
        }
    }
}
