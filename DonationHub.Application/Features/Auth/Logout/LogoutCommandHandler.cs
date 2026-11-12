using DonationHub.Application.Common.Contracts;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Logout
{
    internal class LogoutCommandHandler(IIdentityService _identityService):
        IRequestHandler<LogoutCommand, Result>
    {
        
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.RevokeRefreshTokenAsync(request.RefreshToken);
        }
    
    }
}
