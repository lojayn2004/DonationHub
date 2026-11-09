namespace DonationHub.Domain.Models.Payments
{
    public class ProcessedStripeEvent
    {
        public Guid EventId { get; private set; }
        public string Type { get; private set; }

        public DateTimeOffset ProcessedAt { get; private set; }
    }
}
