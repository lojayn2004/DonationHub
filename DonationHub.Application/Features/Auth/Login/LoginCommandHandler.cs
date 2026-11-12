using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;


namespace DonationHub.Application.Features.Auth.Login
{
    internal class LoginCommandHandler(IIdentityService _identityService, IJwtUtils _jwtUtils):
        IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if(await _identityService.IsAccountLockedAsync(request.Email))
                return Result<AuthResponse>.Failure(AuthErrors.AccountDeactivated);
            var isValidCredentials = await _identityService.ValidateCredentialsAsync(request.Email, request.Password);
            if(!isValidCredentials) 
                return Result<AuthResponse>.Failure(AuthErrors.InValidCredentials);
            return await _identityService.GetAuthResponseAsync(request.Email);
            
            
        }
    }
}
