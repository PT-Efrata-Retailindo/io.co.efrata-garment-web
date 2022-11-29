using FluentScheduler;
using Infrastructure.External.DanLirisClient.Microservice;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DanLiris.Admin.Web.Schedulers
{
    public class JobRegistry : Registry
    {
        public JobRegistry(IServiceProvider serviceProvider)
        {
            Schedule(() =>
            {

                var coreClient = serviceProvider.GetService<ICoreClient>();
                coreClient.SetProduct();
                coreClient.SetUnitDepartments();
                //coreClient.SetMachineSpinning();
                coreClient.SetMachineType();
                coreClient.SetUoms();

                //var weavingClient = serviceProvider.GetService<IWeavingClient>();
                //weavingClient.SetMaterialType();

            }).ToRunNow().AndEvery(1).Hours();
        }
    }
}
