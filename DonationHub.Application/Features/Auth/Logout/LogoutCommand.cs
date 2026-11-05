using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Logout
{
    public record LogoutCommand(string RefreshToken):
        IRequest<Result>
    {
    }
}
