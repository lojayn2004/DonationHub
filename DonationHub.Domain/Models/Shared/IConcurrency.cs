
namespace DonationHub.Domain.Models.Shared
{
    public interface IConcurrency
    {
        byte[] rowVersion { get;  }
    }
}
