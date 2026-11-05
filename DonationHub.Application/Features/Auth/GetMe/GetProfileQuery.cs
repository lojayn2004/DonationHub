using DonationHub.Application.Common.Dtos;
using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.GetMe
{
    public record GetProfileQuery(string Email) : IRequest<Result<UserDto>>
    {
    }
}
