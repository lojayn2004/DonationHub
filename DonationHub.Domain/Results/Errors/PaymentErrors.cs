
namespace DonationHub.Domain.Results.Errors
{
    public static  class PaymentErrors
    {
        public static Error InValidSignature =>
            new Error("Payment.InValidSignature", "Stripe Webhook signature failed", ErrorType.Validation);



    }
}
