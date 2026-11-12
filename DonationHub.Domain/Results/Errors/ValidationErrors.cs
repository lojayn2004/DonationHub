

namespace DonationHub.Domain.Results.Errors
{
    public static class ValidationErrors
    {
        public static Error Falied = new("Validation.Failed", "Input Validation Faliure", ErrorType.Validation);
    }
}
