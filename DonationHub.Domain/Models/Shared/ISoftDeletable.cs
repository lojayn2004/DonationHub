namespace DonationHub.Domain.Models.Shared
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }

        DateTimeOffset? DeletedAtUtc { get; set; }
    }
}
