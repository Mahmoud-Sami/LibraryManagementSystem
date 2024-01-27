using MediatR;

namespace LMS.Business.Commands.Account.Register
{
    public record RegisterCommand(string name, string username, string password, string confirmPassword) 
        : IRequest<Result>
    {
    }
}
