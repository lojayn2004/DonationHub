namespace DonationHub.Domain.Models.Shared
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }

        DateTime? DeletedAtUtc { get; set; }
    }
}
