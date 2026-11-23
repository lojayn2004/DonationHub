

namespace DonationHub.Application.Common.Contracts
{
    public interface ICurrentUser
    {
        //string? Id { get; }
        bool IsAdmin { get; }
    }

}
