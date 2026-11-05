using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace DonationHub.Infrastructure.Data.Configurations
{
    public class ProcessedStripeEventConfiguration : IEntityTypeConfiguration<ProcessedStripeEvent>
    {
        void IEntityTypeConfiguration<ProcessedStripeEvent>.Configure(EntityTypeBuilder<ProcessedStripeEvent> builder)
        {
            builder.ToTable("ProcessedStripeEvents");

            builder.HasKey(p => p.EventId);

            builder.Property(p => p.EventId)
                    .HasMaxLength(150);

           
            builder.Property(p => p.Type)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.ProcessedAt)
                .IsRequired();
        }
    }
}
