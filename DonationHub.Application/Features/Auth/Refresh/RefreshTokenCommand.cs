using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh
{
    public record RefreshTokenCommand(string RefreshToken): IRequest<Result<AuthResponse>>
    {
    }
}