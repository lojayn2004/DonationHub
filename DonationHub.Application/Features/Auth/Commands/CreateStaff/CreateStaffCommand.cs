using DonationHub.Domain.Results;
using MediatR;

namespace DonationHub.Application.Features.Auth.Commands.CreateStaff
{
    public record CreateStaffCommand(string Email,
        string Password,
        string FullName,
        string UserName,
        string? PrefferedLanguage, 
        string Role): IRequest<Result<CreateStaffResponse>>
    {
    }
}
