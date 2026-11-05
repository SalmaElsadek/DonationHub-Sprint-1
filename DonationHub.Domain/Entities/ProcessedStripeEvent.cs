using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DonationHub.Domain.Entities
{
    public class ProcessedStripeEvent
    {
        public ProcessedStripeEvent()
        {
        }

        public string EventId { get; private set; } = default!;
        public string Type { get; private set; } = default!;
        public DateTimeOffset ProcessedAt { get; private set; }

        public static ProcessedStripeEvent Create(string eventId, string type)
        {
            if(string.IsNullOrWhiteSpace(eventId)) 
                throw new ArgumentNullException("Event Id is required",nameof(eventId));
        
            if(string.IsNullOrWhiteSpace(type)) 
                throw new ArgumentNullException("Event Type i required",nameof(type));

            return new ProcessedStripeEvent
            {
                EventId = eventId,
                Type = type,
                ProcessedAt=DateTimeOffset.UtcNow
            };
        }
    }
}
