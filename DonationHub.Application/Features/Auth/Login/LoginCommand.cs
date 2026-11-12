
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Login
{
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>
    {
    }
}
