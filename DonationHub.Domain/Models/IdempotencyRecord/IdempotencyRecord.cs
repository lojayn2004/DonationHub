using DonationHub.Domain.Models.Shared;

namespace DonationHub.Domain.Models.IdempotencyRecord
{
    public class IdempotencyRecord: BaseEntity
    {

        public string RequestHash { get; private set; }

        public int ResponseStatusCode { get; private set; }

        public string ResponseBody { get; private set; }


        public DateTimeOffset ExpiresAt { get; private set; }


        public Guid UserId { get; private set; }

        public static IdempotencyRecord Create()
        {
            throw new NotImplementedException();
        }

    }
}
