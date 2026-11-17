
namespace DonationHub.Domain.Results.Errors
{
    public static class CommonErrors
    {
        public static readonly Error ConcurrencyConflict =
           new Error("ConcurrencyConflict", " Concurrent update detected", ErrorType.Conflict);


        public static readonly Error RateLimited =
           new Error("RateLimited", " Rate limit exceeded", ErrorType.TooManyRequests);


    }
}
