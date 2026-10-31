

namespace DonationHub.Application.Common.Results
{
    public record Error(string Code, string Message, ErrorType Type)
    {
        public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
        public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
        public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.UnAuthorized);
        public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);
    }
}
