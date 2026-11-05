using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Persistence.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
    {
        builder.ToTable("Donations");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.CampaignId)
            .IsRequired();

        builder.OwnsOne(d => d.Amount, money =>
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

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne<Campaign>()
            .WithMany()
            .HasForeignKey(d => d.CampaignId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(d => d.DonorUserId).IsRequired();

        // Easy Searching
        builder.HasIndex(d => d.CampaignId);
    }
}