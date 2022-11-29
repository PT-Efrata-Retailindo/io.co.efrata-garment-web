using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport
{
    public class GarmentRealizationSubconReportQueryHandler : IQueryHandler<GarmentRealizationSubconReportQuery, GarmentRealizationSubconReportListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;

        public GarmentRealizationSubconReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();
           


            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string bcNoOut { get;  set; }
            public DateTimeOffset bcDateOut { get;  set; }
            public double quantityOut { get;  set; }
            public string uomOut { get;  set; }
            public string jobtype { get;  set; }
            public string subconNo { get; set; }
            public string bpjNo { get; set; }
            public DateTimeOffset dueDate { get; set; }
        }

        public async Task<GarmentRealizationSubconReportListViewModel> Handle(GarmentRealizationSubconReportQuery request, CancellationToken cancellationToken)
        {
            GarmentRealizationSubconReportListViewModel listViewModel = new GarmentRealizationSubconReportListViewModel();
            List<GarmentRealizationSubconReportDto> monitoringDtos = new List<GarmentRealizationSubconReportDto>();
            List<GarmentRealizationSubconReportDto> monitoringDtosOut = new List<GarmentRealizationSubconReportDto>();

            var QueryKeluar = from a in garmentSubconCustomsOutRepository.Query
                              join b in garmentSubconCustomsOutItemRepository.Query on a.Identity equals b.SubconCustomsOutId
                              join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
                              where a.SubconContractNo == request.subconcontractNo
                              select new monitoringViewTemp
                              {
                                  bcDateOut = a.CustomsOutDate,
                                  bcNoOut = a.CustomsOutNo,
                                  quantityOut = b.Quantity,
                                  uomOut = c.UomUnit,
                                  jobtype = c.JobType,
                                  subconNo = c.ContractNo,
                                  bpjNo = c.BPJNo,
                                  dueDate = c.DueDate
                              };

            var QueryKeluar2 = from a in garmentSubconContractRepository.Query
                               join b in garmentSubconContractItemRepository.Query on a.Identity equals b.SubconContractId
                               where a.ContractNo == request.subconcontractNo
                               select new monitoringViewTemp
                               {
                                   quantityOut = b.Quantity,
                                   uomOut = b.UomUnit,
                                   jobtype = b.ProductCode + "-" + b.ProductName,
                                   subconNo = a.ContractNo,
                                   bpjNo = a.BPJNo,
                                   dueDate = a.DueDate
                               };

            var QueryKeluar3 = QueryKeluar.Union(QueryKeluar2).AsEnumerable();

            var QueryMasuk = from a in garmentSubconCustomsInRepository.Query
                             join b in garmentSubconCustomsInItemRepository.Query on a.Identity equals b.SubconCustomsInId
                             join c in garmentSubconContractRepository.Query on a.SubconContractId equals c.Identity
                             where a.SubconContractNo == request.subconcontractNo
                             select new monitoringViewTemp
                             {
                                 bcDateOut = a.BcDate,
                                 bcNoOut = a.BcNo,
                                 quantityOut = (double)b.Quantity,
                                 uomOut = c.UomUnit,
                                 jobtype = c.JobType,
                                 subconNo = c.ContractNo,
                                 bpjNo = c.BPJNo,
                                 dueDate = c.DueDate
                             };

            foreach (var i in QueryKeluar3)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate
                };

                monitoringDtosOut.Add(dto);
            }

            foreach (var i in QueryMasuk)
            {
                GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto
                {
                    bcDateOut = i.bcDateOut,
                    bcNoOut = i.bcNoOut,
                    quantityOut = i.quantityOut,
                    uomOut = i.uomOut,
                    jobType = i.jobtype,
                    subconNo = i.subconNo,
                    bpjNo = i.bpjNo,
                    dueDate = i.dueDate
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentRealizationSubconReportDtos = monitoringDtos;
            listViewModel.garmentRealizationSubconReportDtosOUT = monitoringDtosOut;
            return listViewModel;
        }
    }
}
