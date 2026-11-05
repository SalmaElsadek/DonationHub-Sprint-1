using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Domain.Entities;
using DonationHub.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DonationHub.Infrastructure.Data
{
    public class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<AppDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DefaultSeeder");

            try
            {
                string[] roles = { "Admin", "CampaignManager", "Donor" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                    }
                }

                var adminEmail = configuration["SeedSettings:Admin:Email"] ?? "admin@donationhub.com";
                var adminPass = configuration["SeedSettings:Admin:Password"] ?? "AdminPassword123!";

                if (await userManager.FindByEmailAsync(adminEmail) == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "System Admin",
                        EmailConfirmed = true,
                        IsActive = true
                    };
                    var result = await userManager.CreateAsync(adminUser, adminPass);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }

                var managerEmail = configuration["SeedSettings:CampaignManager:Email"] ?? "manager@donationhub.com";
                var managerPass = configuration["SeedSettings:CampaignManager:Password"] ?? "ManagerPassword123!";
                if (await userManager.FindByEmailAsync(managerEmail) == null)
                {
                    var managerUser = new ApplicationUser
                    {
                        UserName = managerEmail,
                        Email = managerEmail,
                        FullName = "Campaign Manager",
                        EmailConfirmed = true,
                        IsActive = true
                    };
                    var result = await userManager.CreateAsync(managerUser, managerPass);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(managerUser, "CampaignManager");
                    }
                }

                var donorEmail = configuration["SeedSettings:Donor:Email"] ?? "donor@donationhub.com";
                var donorPass = configuration["SeedSettings:Donor:Password"] ?? "DonorPassword123!";
                if (await userManager.FindByEmailAsync(donorEmail) == null)
                {
                    var donorUser = new ApplicationUser
                    {
                        UserName = donorEmail,
                        Email = donorEmail,
                        FullName = "Test Donor",
                        EmailConfirmed = true,
                        IsActive = true
                    };
                    var result = await userManager.CreateAsync(donorUser, donorPass);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(donorUser, "Donor");
                    }
                }

                if (!context.Categories.Any())
                {
                    var categories = new List<Category>
                    {
                        Category.Create("Health", "الصحة", "دعم القطاع الصحي والمساعدات الطبية" ,"Health sector support and medical aid",DateTimeOffset.UtcNow),
                        Category.Create("Education", "التعليم", "الدعم التعليمي والمستلزمات المدرسية", "Educational support and school supplies",DateTimeOffset.UtcNow),
                        Category.Create("Relief", "الإغاثة", "الإغاثة العاجلة للأسر المتضررة", "Emergency relief for affected families",DateTimeOffset.UtcNow),
                        Category.Create("Orphans", "الأيتام", "كفالة ورعاية الأيتام", "Sponsorship and care for orphans",DateTimeOffset.UtcNow),
                        Category.Create("Environment", "البيئة", "حماية البيئة والتنمية المستدامة", "Environmental protection and sustainability",DateTimeOffset.UtcNow)
                    };

                    await context.Categories.AddRangeAsync(categories);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
