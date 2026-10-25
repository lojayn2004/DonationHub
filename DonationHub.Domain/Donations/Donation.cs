using DonationHub.Domain.Campaigns;
using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Donations
{
    public class Donation: BaseEntity
    {

        public Money Amount { get; private set; }

        public DonationStatus DonationStatus { get; private set; }

        public DateTime? CompletedAt { get; private set; }

       

        public string? FaliureReson { get; private set; }

        public string? ReceiptNumber { get; private set; }

        public string? ReceiptFileName { get; private set; }

        // relations

        // campaign id
        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }

        // donor user id

        public Guid DonorUserId { get; private set; }

        public ApplicationUser Donor { get; private set; }

        public static Donation Create(Guid campaignId,
                                     Guid donorUserId,
                                     Money donationAmount, 
                                     DateTime utcNow)
        {
            // TODO: enhance exception type
            if (donationAmount.Amount <= 0)
                throw new Exception("Donation Amount Must Be Positive");
            return new Donation
            {
                Id = Guid.NewGuid(),
                CampaignId = campaignId,
                DonorUserId = donorUserId,
                Amount = donationAmount,
                DonationStatus = DonationStatus.PENDING,
                CreatedAt = utcNow
            };
        }


    }
}
