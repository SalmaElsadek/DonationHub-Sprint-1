using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Domain.Enum
{
    public enum PaymentAttemptStatus
    {
        Created,
        Succeeded, 
        Failed, 
        Expired
    }
}
