using LMS.DataAccess.Interfaces;
using LMS.DataAccess.Models;
using MediatR;

namespace LMS.Business.Commands.Account.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Result requestValidation = ValidateRequest(request);
            if (requestValidation.Failed)
                return requestValidation;

            if (await _userRepository.IsExistsAsync(request.username))
                return Result.Error($"Username [{request.username}] already exists");

            User user = new User()
            {
                Name = request.name,
                Username = request.username,
                Password = PasswordHasher.Hash(request.password)
            };

            await _userRepository.RegisterAsync(user);

            return Result.Ok("User registered successfully");
        }

        private Result ValidateRequest(RegisterCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.username))
                return Result.Error("Username field is missing");

            if (string.IsNullOrWhiteSpace(request.password))
                return Result.Error("Password field is missing");

            if (string.IsNullOrWhiteSpace(request.confirmPassword))
                return Result.Error("Confirm password field is missing");

            if (!string.Equals(request.password, request.confirmPassword))
                return Result.Error("Password not match the confirm password field");

            return Result.Ok();
        }
    }
}
