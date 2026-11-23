
namespace DonationHub.Application.Features.Campaigns.Common
{
    public static class CampaignUtils
    {
        public static CampaignDto ToLanguageResult(this CampaignDto campaignDto, 
            bool isAdmin, 
            bool isArabic)
        {
            if (isAdmin)
            {
                campaignDto.Title = null;
                campaignDto.Description = null;
                return campaignDto;
            }
            campaignDto.Title = isArabic ? campaignDto.TitleAr: campaignDto.TitleEn;
            campaignDto.Description = isArabic ? campaignDto.DescriptionAr : campaignDto.DescriptionEn;


            campaignDto.TitleAr = null;
            campaignDto.DescriptionAr = null;
            campaignDto.TitleEn = null;
            campaignDto.DescriptionEn = null;
            return campaignDto;
        }


        public static MainInfoCampginDto ToLanguageResult(this MainInfoCampginDto campaignDto,
           bool isAdmin,
           bool isArabic)
        {
            if (isAdmin)
            {
                campaignDto.Title = null;
                campaignDto.Description = null;
                return campaignDto;
            }
            campaignDto.Title = isArabic ? campaignDto.TitleAr : campaignDto.TitleEn;
            campaignDto.Description = isArabic ? campaignDto.DescriptionAr : campaignDto.DescriptionEn;


            campaignDto.TitleAr = null;
            campaignDto.DescriptionAr = null;
            campaignDto.TitleEn = null;
            campaignDto.DescriptionEn = null;
            return campaignDto;
        }
    }
}
