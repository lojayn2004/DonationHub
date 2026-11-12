
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh
{
    public record RefreshTokenCommand(string RefreshToken): IRequest<Result<AuthResponse>>
    {
    }
}