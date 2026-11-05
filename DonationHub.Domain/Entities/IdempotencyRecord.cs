using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Domain.Entities
{
    public class IdempotencyRecord
    {
        public IdempotencyRecord()
        {
        }

        public string Key { get; private set; } = default!;
        public Guid UserId { get; private set; }
        public string RequestHash { get; private set; } = default!;
        public int ResponseStatusCode { get; private set; }
        public string ResponseBody { get; private set; } = default!;
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset ExpiresAt { get; private set; }

        public static IdempotencyRecord Create(string key, Guid userId, string requestHash,
        int responseStatusCode,string responseBody,TimeSpan addedPeriod)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException(" key is required.", nameof(key));

            if (string.IsNullOrWhiteSpace(requestHash))
                throw new ArgumentException("Request hash is required.", nameof(requestHash));

            return new IdempotencyRecord
            {
                Key= key,
                UserId= userId,
                RequestHash= requestHash,
                ResponseStatusCode= responseStatusCode,
                ResponseBody= responseBody,
                ExpiresAt= DateTimeOffset.UtcNow.Add(addedPeriod),
                CreatedAt=DateTimeOffset.UtcNow
            };
        }
    }
}
