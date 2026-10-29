using System;
using System.Collections.Generic;
using System.Text;

namespace DonationHub.Domain.Models.Payments
{
    public enum PaymentAttemptStatus
    {
        CREATED, 
        SUCCEEDED,
        FAILED, 
        EXPIRED
    }
}
