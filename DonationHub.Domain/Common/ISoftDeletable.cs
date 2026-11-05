using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Domain.Common
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

       public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
        }

        public void UndDelete()
        {
            IsDeleted = false;
            DeletedAt = null;
        }
    }
}
