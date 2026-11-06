using DonationHub.Domain.Exceptions;
using DonationHub.Domain.Models.Campaigns;
using DonationHub.Domain.Models.Shared;

namespace DonationHub.Domain.Models.Donations
{
    public class Donation: BaseEntity
    {

        public Money Amount { get; private set; }

        public DonationStatus DonationStatus { get; private set; }

        public DateTime? CompletedAt { get; private set; }

        public string? FaliureReson { get; private set; }

        public string? ReceiptNumber { get; private set; }

        public string? ReceiptFileName { get; private set; }

        public Guid CampaignId { get; private set; }
        public Campaign Campaign { get; private set; }

        public Guid DonorUserId { get; private set; }

        public static Donation Create(Guid campaignId,
                                     Guid donorUserId,
                                     Money donationAmount,
                                     int minAmount,
                                     int maxAmount,
                                     DateTime utcNow)
        {
           
            if (donationAmount.Amount < 0)
                throw new AmountNotValidException("Donation Amount Must Be Positive");
            if(donationAmount.Amount < minAmount || donationAmount.Amount > maxAmount)
                throw new AmountNotValidException($"Donation Amount Must Be Between {minAmount} and {maxAmount}");

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

        public void UpdateStatus(DonationStatus status)
        {
            if (DonationStatus != DonationStatus.PENDING)
                throw new Exception("Final State Cannot Change");
            DonationStatus = status;
        }


    }
}
