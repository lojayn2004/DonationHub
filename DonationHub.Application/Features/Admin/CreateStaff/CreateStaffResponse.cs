namespace DonationHub.Application.Features.Admin.CreateStaff
{
    public record CreateStaffResponse(Guid Id,
        string FirstName, 
        string LastName, 
        string Email,
        string Role)
    {
    }
}