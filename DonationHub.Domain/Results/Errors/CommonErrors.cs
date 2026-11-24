
namespace DonationHub.Domain.Results.Errors
{
    public static class CommonErrors
    {
        public static readonly Error ConcurrencyConflict =
           new Error("Common.ConcurrencyConflict", " Concurrent update detected", ErrorType.Conflict);


        public static readonly Error RateLimited =
           new Error("Common.RateLimited", " Rate limit exceeded", ErrorType.TooManyRequests);


    }
}
