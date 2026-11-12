
using DonationHub.Application.Features.Auth.Common;
using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Refresh
{
    public record RegisterDonerCommand(string Email, 
        string Password, 
        string FullName,
        string UserName,
        string? PrefferedLanguage): IRequest<Result<AuthResponse>>
    {
    }
}
