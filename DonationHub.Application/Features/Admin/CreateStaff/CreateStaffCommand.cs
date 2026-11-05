using DonationHub.Application.Common.Results;
using MediatR;

namespace DonationHub.Application.Features.Admin.CreateStaff
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
