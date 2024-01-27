using LMS.Business.DTOs;
using MediatR;

namespace LMS.Business.Commands.Account.Login
{
    public record LoginCommand(string username, string password)
        : IRequest<Result<UserDTO>>
    {
    }
}
