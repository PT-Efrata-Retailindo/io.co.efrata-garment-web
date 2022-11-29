using ExtCore.Infrastructure.Actions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Manufactures
{
    public class ServicesRegistrar : IConfigureServicesAction
    {
        public int Priority => 1000;

        public void Execute(IServiceCollection services, IServiceProvider sp)
        {
            //services.AddTransient<IManufactureOrderService, ManufactureOrderService>();
        }
    }
}