

namespace DonationHub.Domain.Results.Errors
{
    public static class DonationErrors
    {
        public static readonly Error NotFound =
        new("Donation.NotFound", "Donation was not found.", ErrorType.NotFound);

        public static readonly Error AmountOutOfRange =
            new Error("Donation.AmountOutOfRange", " Amount outside the DonationOptions range", ErrorType.UnprocessableEntity);



        public static readonly Error InValidState =
          new Error("Donation.InValidState", "Donation already in a final state", ErrorType.Conflict);

    }
}
