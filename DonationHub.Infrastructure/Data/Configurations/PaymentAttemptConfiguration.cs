using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Persistence.Configurations;

public class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
{
    public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
    {
        builder.ToTable("PaymentAttempts");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.StripeCheckoutSessionId)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.StripePaymentIntentId)
            .HasMaxLength(250);

        builder.OwnsOne(p => p.Amount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.FailureCode)
            .HasMaxLength(100);

        builder.HasOne(p => p.Donation)
            .WithMany()
            .HasForeignKey(p => p.DonationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}