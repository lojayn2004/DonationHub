

namespace DonationHub.Application.Common.Contracts
{
    public interface ICurrentLanguage
    {
        string Code { get; }     
        bool IsArabic { get; }
    }
}
