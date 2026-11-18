

namespace DonationHub.Domain.Results
{

    public enum ErrorType
    {
        Validation = 400,
        UnAuthorized = 401,  
        Forbidden = 403,  
        NotFound = 404,
        Conflict = 409,  
        PayloadTooLarge = 413,
        UnprocessableEntity = 422,
        TooManyRequests = 429,
        UnExpected = 500
    }
}
