

namespace DonationHub.Domain.Results.Errors
{
    public class FileErrors
    {
        public static readonly Error InValidType =
            new Error("InValidType", " File Type not allowed", ErrorType.Validation);

        public static readonly Error TooLarge =
           new Error("TooLarge", " File exceedes maximum size", ErrorType.PayloadTooLarge);

    }
}
