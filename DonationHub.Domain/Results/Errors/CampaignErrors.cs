

namespace DonationHub.Domain.Results.Errors
{
    public static class CampaignErrors
    {
        public static readonly Error NotFound =
          new("Campaign.NotFound", "Campaign was not found.", ErrorType.NotFound);

        public static readonly Error NotActive =
            new Error("Campaign.NotActive", " Campaign is completed or active", ErrorType.Conflict);

        public static readonly Error GoalBelowRaised =
           new Error("Campaign.GoalBelowRaised", " Goal cannot be lower than the raised amount", ErrorType.UnprocessableEntity);

        public static readonly Error EndDateInPast =
           new Error("Campaign.EndDateInPast", "End Date Must be in future", ErrorType.UnprocessableEntity);

        public static readonly Error ConcurrencyConflict =
         new Error("Campaign.ConcurrencyConflict", " Concurrent update detected", ErrorType.Conflict);

    }
}
