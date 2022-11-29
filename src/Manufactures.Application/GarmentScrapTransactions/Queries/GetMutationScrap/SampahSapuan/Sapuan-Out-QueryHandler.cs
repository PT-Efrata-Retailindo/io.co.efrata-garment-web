using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.SampahSapuan;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.SampahSapuan
{
    public class Sapuan_Out_QueryHandler : IQueryHandler<Sapuan_Out_Query, ScrapListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
        private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;

        public Sapuan_Out_QueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
            _garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
        }

        class monitoringView
        {
            public string transactionNo { get; set; }
            public DateTimeOffset transactionDate { get; set; }
            public string scrapSourceName { get; set; }
            public double quantity { get; set; }
            public string uomUnit { get; set; }
        }
        public async Task<ScrapListViewModel> Handle(Sapuan_Out_Query request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(0, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(0, 0, 0));

            List<ScrapDto> scrapDtos = new List<ScrapDto>();

            var ScrapTCKecil = (from a in _garmentScrapTransactionRepository.Query
                                join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                                where a.CreatedDate >= dateFrom && a.CreatedDate <= dateTo && a.Deleted == false && b.Deleted == false
                                && a.TransactionType == "OUT" && b.ScrapClassificationName == "SAMPAH SAPUAN"
                                select new monitoringView
                                {
                                    transactionNo = a.TransactionNo,
                                    transactionDate = a.TransactionDate,
                                    scrapSourceName = b.Description,
                                    quantity = b.Quantity,
                                    uomUnit = b.UomUnit,
                                }).GroupBy(x => new { x.transactionNo, x.transactionDate, x.scrapSourceName, x.uomUnit }, (key, group) => new monitoringView
                                {
                                    transactionNo = key.transactionNo,
                                    transactionDate = key.transactionDate,
                                    scrapSourceName = key.scrapSourceName,
                                    quantity = group.Sum(x => x.quantity),
                                    uomUnit = key.uomUnit

                                });

            foreach (var a in ScrapTCKecil)
            {
                scrapDtos.Add(new ScrapDto
                {
                    TransactionNo = a.transactionNo,
                    TransactionDate = a.transactionDate,
                    ScrapSourceName = a.scrapSourceName,
                    Quantity = Math.Round(a.quantity, 2),
                    UomUnit = a.uomUnit,

                });
            }

            ScrapListViewModel scrapListViewModel = new ScrapListViewModel();
            scrapListViewModel.scrapDtos = scrapDtos;

            return scrapListViewModel;
        }
    }
}
