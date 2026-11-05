using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DonationHub.Application.Interfaces;
using DonationHub.Application.Options;
using DonationHub.Application.Services;
using DonationHub.Infrastructure.Data.Interceptors;
using DonationHub.Infrastructure.Identity;
using DonationHub.Infrastructure.Identity.Services;
using DonationHub.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DonationHub.Infrastructure.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<SoftDeleteInterceptor>();

            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var interceptor = serviceProvider.GetRequiredService<SoftDeleteInterceptor>();

                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"))
                  .AddInterceptors(interceptor);
                //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<SeedAdminOptions>()
                .BindConfiguration(SeedAdminOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DonationOptions>()
                .BindConfiguration(DonationOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            return services;
        }
    }
}
