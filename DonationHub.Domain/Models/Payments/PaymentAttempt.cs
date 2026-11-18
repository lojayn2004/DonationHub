using DonationHub.Domain.Models.Donations;
using DonationHub.Domain.Models.Shared;

namespace DonationHub.Domain.Models.Payments
{
    public class PaymentAttempt: BaseEntity
    {

        public Money Amount { get; private set; }

        public PaymentAttemptStatus PaymentAttemptStatus { get; private set; }


        public DateTimeOffset UpdatedAt { get; private set; }

        public string? FaliureCode { get; private set; }

        public string StripeCheckoutSessionId { get; private set; }

        public string? StripePaymentIntentId { get; private set; }
  
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

        public void UpdatePayment(PaymentAttemptStatus status)
        {
            if (PaymentAttemptStatus != PaymentAttemptStatus.CREATED)
                throw new Exception("Final State Cannot be changed");
            PaymentAttemptStatus = status;
        }



    }
}
