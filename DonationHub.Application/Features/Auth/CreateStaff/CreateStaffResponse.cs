namespace DonationHub.Application.Features.Auth.CreateStaff
{
    public record CreateStaffResponse(Guid Id,
        string FirstName, 
        string LastName, 
        string Email,
        string Role)
    {
    }
}