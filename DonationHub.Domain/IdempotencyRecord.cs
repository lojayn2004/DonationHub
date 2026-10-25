

using DonationHub.Domain.Shared;

namespace DonationHub.Domain
{
    public class IdempotencyRecord: BaseEntity
    {

        public string RequestHash { get; private set; }

        public int ResponseStatusCode { get; private set; }

        public string ResponseBody { get; private set; }


        public DateTime ExpiresAt { get; private set; }



        // udser id

    }
}
