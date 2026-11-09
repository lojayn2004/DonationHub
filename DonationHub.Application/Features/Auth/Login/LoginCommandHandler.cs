using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;


namespace DonationHub.Application.Features.Auth.Login
{
    internal class LoginCommandHandler(IIdentityService _identityService, IJwtUtils _jwtUtils):
        IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(await _identityService.IsAccountLockedAsync(request.Email))
                return Result<AuthResponse>.Failure(new Error("AccountLocked", "The account associated with the provided email is locked.", ErrorType.Forbidden));
            var isValidCredentials = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);
            if(!isValidCredentials) 
                return Result<AuthResponse>.Failure(new Error("InvalidCredentials", "The provided credentials are invalid.", ErrorType.UnAuthorized));
            return await _identityService.GetAuthResponseAsync(request.Email);
            
            
        }
    }
}
