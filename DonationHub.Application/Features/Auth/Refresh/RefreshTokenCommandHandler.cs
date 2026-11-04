using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Exceptions;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh
{
    internal class RefreshTokenCommandHandler(IIdentityService _identityService): 
        IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var authModel = await _identityService.RefreshTokenAsync(request.RefreshToken);
                return Result<AuthResponse>.Success(authModel);
            }
            catch(ForbiddenException ex)
            {
                return Result<AuthResponse>.Failure(new Error("AccountLocked", "The account associated with the provided refresh token is locked.", ErrorType.Forbidden));
            }
            
            catch (Exception ex)
            {
                return Result<AuthResponse>.Failure(new Error("InvalidRefreshToken", "The provided refresh token is invalid or expired.", ErrorType.UnAuthorized));
            }
        }
            
    }
}
