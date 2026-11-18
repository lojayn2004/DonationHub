using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Commands.Refresh
{
    internal class RefreshTokenCommandHandler(IIdentityService _identityService): 
        IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            return await _identityService.RefreshTokenAsync(request.RefreshToken);
        }
            
    }
}
