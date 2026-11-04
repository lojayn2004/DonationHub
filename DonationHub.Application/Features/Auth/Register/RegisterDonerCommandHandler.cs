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
            var registerCommand = new RegisterCommand(request.Email, request.Password, request.FullName,
                request.UserName, request.PrefferedLanguage, "Donor");
            var user =  await _identityService.CreateUserAsync(registerCommand);

            var authResponse = await  _identityService.GetAuthResponse(user.Email);
            return Result<AuthResponse>.Success(authResponse);
        }
    }
}
