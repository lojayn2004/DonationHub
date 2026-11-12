using DonationHub.Application.Common.Dtos;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Queries.GetMe
{
    public record GetProfileQuery(string Email) : IRequest<Result<UserDto>>
    {
    }
}
