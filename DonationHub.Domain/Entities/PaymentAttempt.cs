using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Common;
using DonationHub.Domain.Enum;
using DonationHub.Domain.ValueObjects;

namespace DonationHub.Domain.Entities
{
    public class PaymentAttempt:BaseEntity
    {
        public PaymentAttempt()
        {
        }

        public Guid DonationId { get; private set; }
        public Donation Donation { get; private set; } 
        public string StripeCheckoutSessionId { get; private set; } =default!;
        public string? StripePaymentIntentId { get; private set; }
        public Money Amount { get; private set; }
        public PaymentAttemptStatus Status { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }
        public string? FailureCode { get; private set; }


        public static PaymentAttempt Create(Guid donationId,string stripeCheckoutSessionId,Money amount)
        {
            if(donationId==Guid.Empty)
                throw new ArgumentException("Donation Id Can't be null",nameof(donationId));

            if (string.IsNullOrWhiteSpace(stripeCheckoutSessionId))
                throw new ArgumentException("Stripe Checkout Session Id is required", nameof(stripeCheckoutSessionId));

            return new PaymentAttempt
            {
                DonationId = donationId,
                StripeCheckoutSessionId = stripeCheckoutSessionId,
                Amount = amount,
                Status = PaymentAttemptStatus.Created,
            };
        }

        public void MakeItSuccess(string stripePaymentIntentId)
        {
            if (Status == PaymentAttemptStatus.Succeeded) return;

            if (string.IsNullOrWhiteSpace(stripePaymentIntentId))
                throw new ArgumentException("stripe Payment Intent Id is required", nameof(stripePaymentIntentId));

            StripePaymentIntentId = stripePaymentIntentId;
            Status = PaymentAttemptStatus.Succeeded;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public void MakeItFailed(string failureCode, string? stripePaymentIntentId=null)
        {
            if (Status == PaymentAttemptStatus.Succeeded) 
                throw new InvalidOperationException("Can't Fail successful attempt");

            StripePaymentIntentId = stripePaymentIntentId;
            Status = PaymentAttemptStatus.Failed;
            UpdatedAt = DateTimeOffset.UtcNow;
            FailureCode= failureCode;
        }
    }
}
