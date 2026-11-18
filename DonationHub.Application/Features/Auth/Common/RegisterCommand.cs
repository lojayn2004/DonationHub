namespace DonationHub.Application.Features.Auth.Common
{
    public record RegisterCommand(string Email,
        string Password,
        string FullName,
        string UserName,
        string? PrefferedLanguage, 
        string Role)
    {
    }
}
