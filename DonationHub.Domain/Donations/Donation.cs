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

        // donor user id


    }
}
