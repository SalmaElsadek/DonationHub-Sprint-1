using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DonationHub.Application
{
    public static class ApplicationServicesRegister
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(c => { }, typeof(ApplicationServicesRegister).Assembly);
            return services;
        }
    }
}
