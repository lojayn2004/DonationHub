

using DonationHub.Domain.Results.Errors;

namespace DonationHub.Domain.Results
{
    public record Error(string Code, string Message, ErrorType Type)
    {
        public static Error NotFound(string code, string message) => new(code, message, ErrorType.NotFound);
        public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);
        public static Error Unauthorized(string code, string message) => new(code, message, ErrorType.UnAuthorized);
        public static Error Forbidden(string code, string message) => new(code, message, ErrorType.Forbidden);
    }

    public record ErrorDetails : Error
    {
        public IReadOnlyDictionary<string, string[]> Details { get; }
        public ErrorDetails(string Code, 
                            string Message,
                            ErrorType Type,
                            IReadOnlyDictionary<string, string[]> details) : base(Code, Message, Type)
        {
            Details = details;
        }

        public static Error Validation(IReadOnlyDictionary<string, string[]> details)
         => new ErrorDetails(ValidationErrors.Falied.Code, ValidationErrors.Falied.Message, ValidationErrors.Falied.Type, details);
    }
}
