using LMS.Business.DTOs;
using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Models;
using MediatR;

namespace LMS.Business.Commands.Account.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<UserDTO>>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Result requestValidation = ValidateRequest(request);
            if (requestValidation.Failed)
                return requestValidation;

            User? user = await _userRepository.GetUserAsync(request.username, PasswordHasher.Hash(request.password));
            if (user is null)
                return Result.Error($"Invalid credentials");

            UserDTO userDTO = new(user);
            return Result.Ok(userDTO, "Logged successfully");
        }

        private Result ValidateRequest(LoginCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.username))
                return Result.Error("Username field is missing");

            if (string.IsNullOrWhiteSpace(request.password))
                return Result.Error("Password field is missing");

            return Result.Ok();
        }

    }
}
