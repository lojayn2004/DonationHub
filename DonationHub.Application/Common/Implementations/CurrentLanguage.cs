
using DonationHub.Application.Common.Contracts;
using System.Globalization;

namespace DonationHub.Application.Common.Implementations
{
    public class CurrentLanguage : ICurrentLanguage
    {
        public string Code => CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        public bool IsArabic => Code == "ar";
    }
}
