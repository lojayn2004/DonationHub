using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Logout
{
    internal class LogoutCommandHandler(IIdentityService _identityService):
        IRequestHandler<LogoutCommand, Result>
    {
        
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RevokeRefreshToken(request.RefreshToken);
            if (!result)
                return Result.Failure(new Error("UnAuthorized", "UnAuthorized", ErrorType.UnAuthorized));
            
            return Result.Success();
        }
    
    }
}
