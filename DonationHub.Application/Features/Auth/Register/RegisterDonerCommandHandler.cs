using DonationHub.Application.Features.Auth.Common;
using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Common;
using MediatR;


namespace DonationHub.Application.Features.Auth.Refresh
{
    internal class RegisterDonerCommandHandler(IIdentityService _identityService, IJwtUtils _jwtUtils)
        :IRequestHandler<RegisterDonerCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RegisterDonerCommand request, CancellationToken cancellationToken)
        {
            if(await _identityService.EmailExistsAsync(request.Email))
                return Result<AuthResponse>.Failure(new Error("UserAlreadyExists", "A user with the provided email already exists.", ErrorType.Conflict));

            var registerCommand = new RegisterCommand(request.Email, request.Password, request.FullName,
                request.UserName, request.PrefferedLanguage, "Donor");

            var userResult = await _identityService.CreateUserAsync(registerCommand);
            if (!userResult.IsSuccess)
                return Result<AuthResponse>.Failure(userResult.Error!);

            return await _identityService.GetAuthResponseAsync(userResult.Value!.Email);
        }
    }
}
