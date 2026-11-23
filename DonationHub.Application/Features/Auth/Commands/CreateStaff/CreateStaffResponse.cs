namespace DonationHub.Application.Features.Auth.Commands.CreateStaff
{
    public record CreateStaffResponse(Guid Id,
        string FullName, 
        string UserName,
        string Email,
        string PreferredLanguage)
    {
    }
}