using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;


namespace DonationHub.Application.Features.Auth.Refresh
{
    internal class RegisterDonerCommandHandler(IIdentityService _identityService, IJwtUtils _jwtUtils)
        :IRequestHandler<RegisterDonerCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RegisterDonerCommand request, CancellationToken cancellationToken)
        {
            var userId =  await _identityService.CreateDonorAsync(request);
            var authResponse = new AuthResponse()
            {
                Token = await _jwtUtils.GenerateJwtTokenAsync(userId)
            };
            return Result<AuthResponse>.Success(authResponse);
        }
    }
}
