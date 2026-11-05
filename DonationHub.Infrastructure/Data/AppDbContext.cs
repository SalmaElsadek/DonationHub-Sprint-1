using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DonationHub.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public  AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
            public DbSet<Campaign> Campaigns { get; set; }
            public DbSet<Category> Categories { get; set; }
            public DbSet<Donation> Donations { get; set; }
            public DbSet<PaymentAttempt> PaymentAttempts { get; set; }
            public DbSet<ProcessedStripeEvent> ProcessedStripeEvents {  get; set; }
            public DbSet<IdempotencyRecord> IdempotencyRecords {  get; set; }

            public DbSet<RefreshToken> RefreshTokens { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<Campaign>().HasQueryFilter(c => !c.IsDeleted);
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            }  
    }
}
