using DanLiris.Admin.Web;
using ExtCore.Infrastructure.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;

namespace Infrastructure
{
    public class ServicesRegistrar : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection services, IServiceProvider sp)
        {
            services.AddScoped(c => new WorkContext() { CurrentUser = "System" });
            services.AddScoped<IWebApiContext>(c => c.GetRequiredService<WorkContext>());
            services.AddScoped<IWorkContext>(c => c.GetRequiredService<WorkContext>());
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}