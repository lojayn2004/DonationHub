using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;

namespace DonationHub.Application.Features.Auth.GetMe
{
    internal class GetProfileQueryHandler(IIdentityService _identityService): IRequestHandler<GetProfileQuery, Result<UserDto>>
    {
        public async Task<Result<UserDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
             var user = await _identityService.FindUserByEmailAsync(request.Email);
             if (user == null)
                 return Result<UserDto>.Failure(AuthErrors.InValidCredentials);

             return Result<UserDto>.Success(user);
        }
    }
}
