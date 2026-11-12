using DonationHub.Application.Common.Contracts;
using DonationHub.Application.Common.Dtos;
using DonationHub.Domain.Results;
using DonationHub.Domain.Results.Errors;
using MediatR;

namespace DonationHub.Application.Features.Auth.Queries.GetMe
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
