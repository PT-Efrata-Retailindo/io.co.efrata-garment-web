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

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap
{
    public class GetMutationScrapQueryHandler : IQueryHandler<GetMutationScrapQuery, GetMutationScrapListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
        private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;
        private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;
        //private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;
        //private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;
        //private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;

        public GetMutationScrapQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
            _garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
            _garmentScrapClassificationRepository = storage.GetRepository<IGarmentScrapClassificationRepository>();
            //_garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
            //_garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
            //_garmentScrapStockRepository = storage.GetRepository<IGarmentScrapStockRepository>();

        }

        class monitoringView
        {
            public string classificationCode { get; set; }
            public string classificationName { get; set; }
            public string unitQtyName { get; set; }
            public double saldoAwal { get; set; }
            public double pemasukan { get; set; }
            public double pengeluaran { get; set; }
            public double penyesuaian { get; set; }
            public double stockOpname { get; set; }
            public double selisih { get; set; }
            public double saldoAkhir { get; set; }
        }

        public async Task<GetMutationScrapListViewModel> Handle(GetMutationScrapQuery request, CancellationToken cancellationToken)
        {
            //DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
            //DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(0, 0, 0));
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(0, 0, 0));

            List<GetMutationScrapDto> getMutationScrapDtos = new List<GetMutationScrapDto>();

            var CodeScrap = new List<string> { "ZB05", "ZA59" };

            var SAScrapIN = (from a in _garmentScrapTransactionRepository.Query
                             join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                             join c in _garmentScrapClassificationRepository.Query on b.ScrapClassificationId equals c.Identity
                             where a.CreatedDate < dateFrom && a.Deleted == false && b.Deleted == false
                             && CodeScrap.Contains(c.Code) && a.TransactionType == "IN"
                             select new monitoringView
                             {
                                 classificationCode = c.Code,
                                 classificationName = c.Name,
                                 saldoAwal = b.Quantity,
                                 pemasukan = 0,
                                 pengeluaran = 0,
                                 penyesuaian = 0,
                                 saldoAkhir = 0,
                                 selisih = 0,
                                 stockOpname = 0,
                                 unitQtyName = b.UomUnit
                             }).GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
                             {
                                 classificationCode = key.classificationCode,
                                 classificationName = key.classificationName,
                                 saldoAwal = group.Sum(x => x.saldoAwal),
                                 pemasukan = group.Sum(x => x.pemasukan),
                                 pengeluaran = group.Sum(x => x.pengeluaran),
                                 penyesuaian = group.Sum(x => x.penyesuaian),
                                 saldoAkhir = group.Sum(x => x.saldoAkhir),
                                 selisih = group.Sum(x => x.selisih),
                                 stockOpname = group.Sum(x => x.stockOpname),
                                 unitQtyName = key.unitQtyName

                             });

            var SAScrapOut = (from a in _garmentScrapTransactionRepository.Query
                             join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                             join c in _garmentScrapClassificationRepository.Query on b.ScrapClassificationId equals c.Identity
                             where a.CreatedDate < dateFrom && a.Deleted == false && b.Deleted == false
                             && CodeScrap.Contains(c.Code) && a.TransactionType == "OUT"
                             select new monitoringView
                             {
                                 classificationCode = c.Code,
                                 classificationName = c.Name,
                                 saldoAwal = -b.Quantity,
                                 pemasukan = 0,
                                 pengeluaran = 0,
                                 penyesuaian = 0,
                                 saldoAkhir = 0,
                                 selisih = 0,
                                 stockOpname = 0,
                                 unitQtyName = b.UomUnit
                             }).GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
                             {
                                 classificationCode = key.classificationCode,
                                 classificationName = key.classificationName,
                                 saldoAwal = group.Sum(x => x.saldoAwal),
                                 pemasukan = group.Sum(x => x.pemasukan),
                                 pengeluaran = group.Sum(x => x.pengeluaran),
                                 penyesuaian = group.Sum(x => x.penyesuaian),
                                 saldoAkhir = group.Sum(x => x.saldoAkhir),
                                 selisih = group.Sum(x => x.selisih),
                                 stockOpname = group.Sum(x => x.stockOpname),
                                 unitQtyName = key.unitQtyName

                             });
            var SA = SAScrapIN.Concat(SAScrapOut).AsEnumerable();
            var SaldoAwal = SA.GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
            {
                classificationCode = key.classificationCode,
                classificationName = key.classificationName,
                saldoAwal = group.Sum(x => x.saldoAwal),
                pemasukan = group.Sum(x => x.pemasukan),
                pengeluaran = group.Sum(x => x.pengeluaran),
                penyesuaian = group.Sum(x => x.penyesuaian),
                saldoAkhir = group.Sum(x => x.saldoAkhir),
                selisih = group.Sum(x => x.selisih),
                stockOpname = group.Sum(x => x.stockOpname),
                unitQtyName = key.unitQtyName

            });

            var FilterdScrapIN = (from a in _garmentScrapTransactionRepository.Query
                             join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                             join c in _garmentScrapClassificationRepository.Query on b.ScrapClassificationId equals c.Identity
                             where a.CreatedDate.Date.Date >= dateFrom.Date && a.CreatedDate.Date.Date <= dateTo.Date
                             && a.Deleted == false && b.Deleted == false
                             && CodeScrap.Contains(c.Code) && a.TransactionType == "IN"
                             select new monitoringView
                             {
                                 classificationCode = c.Code,
                                 classificationName = c.Name,
                                 saldoAwal = 0,
                                 pemasukan = b.Quantity,
                                 pengeluaran = 0,
                                 penyesuaian = 0,
                                 saldoAkhir = 0,
                                 selisih = 0,
                                 stockOpname = 0,
                                 unitQtyName = b.UomUnit
                             }).GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
                             {
                                 classificationCode = key.classificationCode,
                                 classificationName = key.classificationName,
                                 saldoAwal = group.Sum(x => x.saldoAwal),
                                 pemasukan = group.Sum(x => x.pemasukan),
                                 pengeluaran = group.Sum(x => x.pengeluaran),
                                 penyesuaian = group.Sum(x => x.penyesuaian),
                                 saldoAkhir = group.Sum(x => x.saldoAkhir),
                                 selisih = group.Sum(x => x.selisih),
                                 stockOpname = group.Sum(x => x.stockOpname),
                                 unitQtyName = key.unitQtyName

                             });

            var FilterdScrapOut = (from a in _garmentScrapTransactionRepository.Query
                              join b in _garmentScrapTransactionItemRepository.Query on a.Identity equals b.ScrapTransactionId
                              join c in _garmentScrapClassificationRepository.Query on b.ScrapClassificationId equals c.Identity
                              where a.CreatedDate >= dateFrom && a.CreatedDate <= dateTo && a.Deleted == false && b.Deleted == false
                              && CodeScrap.Contains(c.Code) && a.TransactionType == "OUT"
                              select new monitoringView
                              {
                                  classificationCode = c.Code,
                                  classificationName = c.Name,
                                  saldoAwal = 0,
                                  pemasukan = 0,
                                  pengeluaran = b.Quantity,
                                  penyesuaian = 0,
                                  saldoAkhir = 0,
                                  selisih = 0,
                                  stockOpname = 0,
                                  unitQtyName = b.UomUnit
                              }).GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
                              {
                                  classificationCode = key.classificationCode,
                                  classificationName = key.classificationName,
                                  saldoAwal = group.Sum(x => x.saldoAwal),
                                  pemasukan = group.Sum(x => x.pemasukan),
                                  pengeluaran = group.Sum(x => x.pengeluaran),
                                  penyesuaian = group.Sum(x => x.penyesuaian),
                                  saldoAkhir = group.Sum(x => x.saldoAkhir),
                                  selisih = group.Sum(x => x.selisih),
                                  stockOpname = group.Sum(x => x.stockOpname),
                                  unitQtyName = key.unitQtyName

                              });
            var SAkhir = SaldoAwal.Concat(FilterdScrapIN).Concat(FilterdScrapOut).AsEnumerable();
            var Saldokhir = SAkhir.GroupBy(x => new { x.classificationCode, x.classificationName, x.unitQtyName }, (key, group) => new monitoringView
            {
                classificationCode = key.classificationCode,
                classificationName = key.classificationName,
                saldoAwal = group.Sum(x => x.saldoAwal),
                pemasukan = group.Sum(x => x.pemasukan),
                pengeluaran = group.Sum(x => x.pengeluaran),
                penyesuaian = group.Sum(x => x.penyesuaian),
                saldoAkhir = group.Sum(x => x.saldoAwal) + group.Sum(x => x.pemasukan) - group.Sum(x => x.pengeluaran),
                selisih = group.Sum(x => x.selisih),
                stockOpname = group.Sum(x => x.stockOpname),
                unitQtyName = key.unitQtyName

            });

            foreach (var a in Saldokhir)
            {
                getMutationScrapDtos.Add(new GetMutationScrapDto
                {
                    ClassificationCode = a.classificationCode,
                    ClassificationName = a.classificationName,
                    SaldoAwal = Math.Round(a.saldoAwal, 2),
                    Pemasukan = Math.Round(a.pemasukan, 2),
                    Pengeluaran = Math.Round(a.pengeluaran, 2),
                    Penyesuaian = Math.Round(a.penyesuaian, 2),
                    SaldoAkhir = Math.Round(a.saldoAkhir, 2),
                    Selisih = a.selisih,
                    StockOpname = a.stockOpname,
                    UnitQtyName = a.unitQtyName
                });
            }

            GetMutationScrapListViewModel getMutationScrapListViewModel = new GetMutationScrapListViewModel();

            getMutationScrapListViewModel.garmentMonitorings = getMutationScrapDtos;

            return getMutationScrapListViewModel;

        }
    }
}
