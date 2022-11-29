using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using Manufactures.Domain.Shared.ValueObjects;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport
{
    public class GarmentSubconContactReportQueryHandler : IQueryHandler<GarmentSubconContactReportQuery, GarmentSubconContactReportListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        //private readonly IGarmentSubconCustomsInRepository garmentSubconCustomsInRepository;
        //private readonly IGarmentSubconCustomsInItemRepository garmentSubconCustomsInItemRepository;
        //private readonly IGarmentSubconCustomsOutRepository garmentSubconCustomsOutRepository;
        //private readonly IGarmentSubconCustomsOutItemRepository garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconContractRepository garmentSubconContractRepository;
        //private readonly IGarmentSubconContractItemRepository garmentSubconContractItemRepository;

        public GarmentSubconContactReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            //garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            //garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
            //garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            //garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            //garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();



            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string ContractType { get; set; }
            public string ContractNo { get; set; }
            public string AgreementNo { get; set; }
            public int SupplierId { get; set; }
            public string SupplierCode { get; set; }
            public string SupplierName { get; set; }
            public string JobType { get; set; }
            public string BPJNo { get; set; }
            public string FinishedGoodType { get; set; }
            public double Quantity { get; set; }
            public DateTimeOffset DueDate { get; set; }
            public DateTimeOffset ContractDate { get; set; }
            public bool IsUsed { get; set; }
            public int BuyerId { get; set; }
            public string BuyerCode { get; set; }
            public string BuyerName { get; set; }
            public string SubconCategory { get; set; }
            public int UomId { get; set; }
            public string UomUnit { get; set; }
            public string SKEPNo { get; set; }
            public DateTimeOffset AgreementDate { get; set; }
        }

        public async Task<GarmentSubconContactReportListViewModel> Handle(GarmentSubconContactReportQuery request, CancellationToken cancellationToken)
        {
            GarmentSubconContactReportListViewModel listViewModel = new GarmentSubconContactReportListViewModel();
            List<GarmentSubconContactReportDto> monitorincontact = new List<GarmentSubconContactReportDto>();

            //if (request.supplierNo != null)
            //{ 

            var result = from a in garmentSubconContractRepository.Query
                         where a.SupplierId == (request.supplierNo != 0 ? request.supplierNo : a.SupplierId)
                         && a.ContractType == (string.IsNullOrWhiteSpace(request.contractType) ? a.ContractType : request.contractType)
                         && a.Deleted == false
                         && a.ContractDate >= request.dateFrom.Date
                         && a.ContractDate <= request.dateTo.Date
                         select new monitoringViewTemp
                         {
                             ContractType = a.ContractType,
                             ContractNo = a.ContractNo,
                             AgreementNo = a.AgreementNo,
                             SupplierId = a.SupplierId,
                             SupplierCode = a.SupplierCode,
                             SupplierName = a.SupplierName,
                             JobType = a.JobType,
                             BPJNo = a.BPJNo,
                             FinishedGoodType = a.FinishedGoodType,
                             Quantity = a.Quantity,
                             DueDate = a.DueDate,
                             ContractDate = a.ContractDate,
                             IsUsed = a.IsUsed,
                             BuyerId = a.BuyerId,
                             BuyerName = a.BuyerName,
                             BuyerCode = a.BuyerCode,
                             SubconCategory = a.SubconCategory,
                             UomId = a.UomId,
                             UomUnit = a.UomUnit,
                             SKEPNo = a.SKEPNo,
                             AgreementDate = a.AgreementDate
                         };

            foreach (var i in result)
            {
                GarmentSubconContactReportDto report = new GarmentSubconContactReportDto
                {
                    ContractType = i.ContractType,
                    ContractNo = i.ContractNo,
                    AgreementNo = i.AgreementNo,
                    SupplierId = i.SupplierId,
                    SupplierCode = i.SupplierCode,
                    SupplierName = i.SupplierName,
                    JobType = i.JobType,
                    BPJNo = i.BPJNo,
                    FinishedGoodType = i.FinishedGoodType,
                    Quantity = i.Quantity,
                    DueDate = i.DueDate,
                    ContractDate = i.ContractDate,
                    IsUsed = i.IsUsed,
                    BuyerId = i.BuyerId,
                    BuyerName = i.BuyerName,
                    BuyerCode = i.BuyerCode,
                    SubconCategory = i.SubconCategory,
                    UomId = i.UomId,
                    UomUnit = i.UomUnit,
                    SKEPNo = i.SKEPNo,
                    AgreementDate = i.AgreementDate
                };

                monitorincontact.Add(report);
            }

            double totalQty = result.Sum(b => b.Quantity);

            listViewModel.garmentSubconContactReportDto = monitorincontact;

            return listViewModel ;

        }
    }
}
