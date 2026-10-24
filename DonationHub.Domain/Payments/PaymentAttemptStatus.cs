using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Payments
{
    public enum PaymentAttemptStatus
    {
        CREATED, 
        SUCCEEDED,
        FAILED, 
        EXPIRED
    }
}
