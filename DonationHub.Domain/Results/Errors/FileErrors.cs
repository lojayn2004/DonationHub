

namespace DonationHub.Domain.Results.Errors
{
    public class FileErrors
    {
        public static readonly Error InValidType =
            new Error("File.InValidType", " File Type not allowed", ErrorType.Validation);

        public static readonly Error TooLarge =
           new Error("File.TooLarge", " File exceedes maximum size", ErrorType.PayloadTooLarge);

    }
}
