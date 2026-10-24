using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Categories
{
    public class Category : BaseEntity
    {
        public string NameAr { get; private set; }

        public string NameEn { get; private set; }

        public string DescriptionAr { get; private set; }

        public string DescriptionEn { get; private set; }

        public string? BannerFileName { get; private set; }
        public DateTime UpdateAt { get; private set; }


        // relation 
        //category id


        // created by user id 

        // row version



    }
}
