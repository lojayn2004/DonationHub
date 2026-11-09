using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Shared;

namespace DonationHub.Domain.Models.Categories
{
    public class Category : BaseEntity, ISoftDeletable
    {
        public string NameAr { get; private set; }

        public string NameEn { get; private set; }

        public string DescriptionAr { get; private set; }

        public string DescriptionEn { get; private set; }

        public DateTimeOffset UpdateAt { get; private set; }

        public List<Campaign> Campaigns { get; private set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAtUtc { get; set; }

        public static Category CreateCategory(string nameAr, string nameEn,
                                              string descriptionAr,
                                              string descriptionEn,
                                              DateTimeOffset utcNow)
        {




            return new Category()
            {
                Id = Guid.NewGuid(),
                NameAr = nameAr,
                NameEn = nameEn,
                DescriptionAr = descriptionAr,
                DescriptionEn = descriptionEn,
                CreatedAt = utcNow,
                UpdateAt = utcNow
            };

        }

        public void Update(string nameAr, string nameEn, string descAr, string descEn, DateTimeOffset utcNow)
        {
            NameAr = nameAr; NameEn = nameEn;
            DescriptionAr = descAr; DescriptionEn = descEn;
            UpdateAt = utcNow;
        }
    }
}
