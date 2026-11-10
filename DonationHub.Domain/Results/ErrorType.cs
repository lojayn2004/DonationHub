

namespace DonationHub.Domain.Results
{
    public enum ErrorType
    {
        Validation,
        NotFound, 
        Conflict,
        UnAuthorized,
        Forbidden, 
        UnExpected,
        
        PayloadTooLarge,
        UnprocessableEntity,

        TooManyRequests
    }
}
