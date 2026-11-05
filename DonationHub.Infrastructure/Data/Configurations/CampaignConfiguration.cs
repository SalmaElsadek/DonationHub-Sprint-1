using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
    {
        void IEntityTypeConfiguration<Campaign>.Configure(EntityTypeBuilder<Campaign> builder)
        {
            builder.ToTable("Campaign");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.TitleAr)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(c => c.TitleEn)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(c => c.DescriptionAr)
                .HasMaxLength(2000);

            builder.Property(c => c.DescriptionEn)
                .HasMaxLength(2000);

            builder.OwnsOne(c => c.GoalAmount, money =>
            {
                money.Property(m => m.Amount)
                    .HasColumnName("GoalAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                money.Property(m => m.Currency)
                    .HasColumnName("GoalCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
            builder.Property(c => c.RaisedAmount)
                    .HasColumnType("decimal(18,2)");

            builder.Property(m => m.CreatedByUserId).IsRequired();

            builder.HasOne(c => c.Category)
                .WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(c => c.RowVersion).IsRowVersion();

            builder.Property(c => c.IsDeleted).IsRequired();
        }
    }
}
