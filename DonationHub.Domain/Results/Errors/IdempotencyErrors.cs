
namespace DonationHub.Domain.Results.Errors
{
    public static  class IdempotencyErrors
    {
        public static Error KeyReusedWithDifferentRequest =>
           new Error("Payment.KeyReusedWithDifferentRequest", "Same Idempotency-Key with a different body", ErrorType.UnprocessableEntity);

    }
}
