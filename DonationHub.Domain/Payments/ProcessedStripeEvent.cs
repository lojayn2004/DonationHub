namespace DonationHub.Domain.Payments
{
    public class ProcessedStripeEvent
    {
        public Guid EventId { get; private set; }
        public string Type { get; private set; }

        public DateTime ProcessedAt { get; private set; }
    }
}
