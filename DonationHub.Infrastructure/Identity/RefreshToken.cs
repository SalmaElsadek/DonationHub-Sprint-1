using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;
using DonationHub.Infrastructure.Identity;

namespace DonationHub.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public RefreshToken()
        {
        }

        public string TokenHash { get; private set; } = default!;
        public DateTimeOffset ExpiredAt { get; private set; }
        public DateTimeOffset? RevokedAt { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsActive {  get; private set; }
        public ApplicationUser User { get; private set; } = null!;

        public static RefreshToken Create(Guid userId, string tokenHash, DateTimeOffset expiredAt)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User Id Can't be null", nameof(userId));

            if (string.IsNullOrWhiteSpace(tokenHash))
                throw new ArgumentException("Token Hash required", nameof(tokenHash));

            if (expiredAt <= DateTimeOffset.UtcNow)
                throw new ArgumentException("Expire date must be in the future", nameof(expiredAt));

            return new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                ExpiredAt = expiredAt,
                IsActive = true
            };
        }

        public void Revoke(Guid? replacedByTokenId=null)
        {
            if (RevokedAt != null) return;

            RevokedAt = DateTimeOffset.UtcNow;
            ReplacedByTokenId = replacedByTokenId;
            IsActive = false;
        }
    }
}
