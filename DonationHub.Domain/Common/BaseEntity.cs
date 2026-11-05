using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    }
}
