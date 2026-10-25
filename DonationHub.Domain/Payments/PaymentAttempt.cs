using DonationHub.Domain.Donations;
using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Payments
{
    public class PaymentAttempt: BaseEntity
    {

        public Money Amount { get; private set; }

        public PaymentAttemptStatus PaymentAttemptStatus { get; private set; }


        public DateTime UpdatedAt { get; private set; }

        public string? FaliureCode { get; private set; }

        public string StripeCheckoutSessionId { get; private set; }

        public string? StripePaymentIntentId { get; private set; }
        // relations


        // donation id 
        public Guid DonationId { get; private set; }

        public Donation Donation { get; private set; }

        public static PaymentAttempt Create(Guid donationId,
                                            string stripeCheckoutSessionId,
                                            Money amount,
                                            DateTime utcNow)
        {
            // TODO: Enhance exception thown
            
            if (amount.Amount <= 0)
                throw new Exception("Payment amount must be positive.");

            return new PaymentAttempt
            {
                Id = Guid.NewGuid(),
                DonationId = donationId,
                StripeCheckoutSessionId = stripeCheckoutSessionId,
                Amount = amount,
                PaymentAttemptStatus = PaymentAttemptStatus.CREATED,   
                CreatedAt = utcNow,
                UpdatedAt = utcNow
            };
        }



    }
}
