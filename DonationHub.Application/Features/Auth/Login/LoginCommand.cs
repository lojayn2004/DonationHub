using DonationHub.Application.Common.Results;
using DonationHub.Application.Features.Auth.Common;
using MediatR;

namespace DonationHub.Application.Features.Auth.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>
    {
    }
}
