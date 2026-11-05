using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Persistence.Configurations;

public class IdempotencyRecordConfiguration : IEntityTypeConfiguration<IdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
    {
        builder.ToTable("IdempotencyRecords");

        builder.HasKey(i => i.Key);

        builder.Property(i => i.Key)
            .HasMaxLength(100);

        builder.Property(i => i.UserId)
            .IsRequired();

        builder.Property(i => i.RequestHash)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(i => i.ResponseStatusCode)
            .IsRequired();

        builder.Property(i => i.ResponseBody)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.ExpiresAt)
            .IsRequired();

        // To make it easy when delete to clean
        builder.HasIndex(i => i.ExpiresAt);
    }
}