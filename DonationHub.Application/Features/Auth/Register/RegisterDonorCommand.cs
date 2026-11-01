

using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Register
{
    public record RegisterDonorCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string PhoneNumber,
        string? Address,
        string? ProfileImageUrl,
        string PreferredLanguage) : IRequest<Result<RegisterDonorResponse>>
    {
    }
}
