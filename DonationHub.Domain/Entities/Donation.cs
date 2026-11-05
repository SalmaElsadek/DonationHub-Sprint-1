using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;
using DonationHub.Domain.Enum;
using DonationHub.Domain.ValueObjects;

namespace DonationHub.Domain.Entities
{
    public class Donation :BaseEntity
    {
        public Donation()
        {
        }

        public Guid CampaignId { get; private set; }
        public Guid DonorUserId { get; private set; }
        public Money Amount { get; private set; }
        public DonationStatus Status { get; private set; }
        public DateTimeOffset? CompletedAt { get; private set; }
        public string? FailureReason { get; private set; } = default!;
        public string? ReceiptNumber { get; private set; } = default!;
        public string? ReceiptFileName { get; private set; } = default!;


        public static Donation Create(Guid campaignId,Guid donorUserId,Money amount)
        {
            if (donorUserId == Guid.Empty)
                throw new ArgumentException("User Id Can't be null", nameof(donorUserId));
            return new Donation() { 
                CampaignId=campaignId,
                DonorUserId=donorUserId,
                Amount=amount,
                Status=DonationStatus.Pending,
            };

        }
        public void MakeItSucceeded(string receiptNumber,string receiptFileName)
        {
            if (Status == DonationStatus.Succeeded || Status == DonationStatus.Failed)
                throw new InvalidOperationException("A donation in a final state cannot be Updated.");

            Status =DonationStatus.Succeeded;
            ReceiptNumber=receiptNumber;
            ReceiptFileName = receiptFileName;
            CompletedAt=DateTimeOffset.UtcNow;
        }

        public void MakeItFailed(string failureReason)
        {
            if (Status == DonationStatus.Succeeded || Status == DonationStatus.Failed)
                throw new InvalidOperationException("A donation in a final state cannot be Updated.");

            Status = DonationStatus.Failed;
            FailureReason=failureReason;
            CompletedAt = DateTimeOffset.UtcNow;
        }
    }
}
