using DonationHub.Domain.Shared;

namespace DonationHub.Domain.Payments
{
    public class PaymentAttempt: BaseEntity
    {

        public Money Amount { get; private set; }

        public PaymentAttemptStatus paymentAttemptStatus { get; private set; }


        public DateTime UpdatedAt { get; private set; }

        public string? FaliureCode { get; private set; }


        public string StripeCheckoutSessionId { get; private set; }

        public string? StripePaymentIntentId { get; private set; }
        // relations


        // donation id 


    }
}
