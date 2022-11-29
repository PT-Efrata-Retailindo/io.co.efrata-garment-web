using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using System.Linq;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Net.Http;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using System.Threading;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring
{
    public class GetSampleCuttingMonitoringQueryHandler : IQueryHandler<GetSampleCuttingMonitoringQuery, GarmentSampleCuttingMonitoringViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository GarmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository GarmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingInRepository GarmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository GarmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository GarmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleAvalComponentRepository GarmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository GarmentSampleAvalComponentItemRepository;
        private readonly IGarmentSamplePreparingRepository GarmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository GarmentSamplePreparingItemRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;
        //private readonly IGarmentBalanceCuttingRepository garmentBalanceCuttingRepository;

        public GetSampleCuttingMonitoringQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            GarmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            GarmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            GarmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            GarmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            GarmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            GarmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            GarmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            GarmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            GarmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
            //garmentBalanceCuttingRepository = storage.GetRepository<IGarmentBalanceCuttingRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();

        }


        //NEW VIEW MODEL
        class ViewFC
        {
            public string RO { get; internal set; }
            public double Total { get; internal set; }
        }

        //NEW VIEW MODEL
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal Total { get; internal set; }
        }
        public async Task<GarmentSampleCuttingMonitoringViewModel> Handle(GetSampleCuttingMonitoringQuery request, CancellationToken cancellationToken)
        {
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);

			var sumbasicPrice = (from a in GarmentSamplePreparingRepository.Query
                                 join b in GarmentSamplePreparingItemRepository.Query
                                 on a.Identity equals b.GarmentSamplePreparingId
                                 select new
                                 {
                                     a.RONo,
                                     b.BasicPrice
                                 })
                        .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                        {
                            RO = key.RONo,
                            //BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                            //Count = group.Count(),
                            Total = Convert.ToDecimal(group.Sum(s => s.BasicPrice)) / group.Count()
                        });
            
            var sumFCs = (from a in (from cut in GarmentSampleCuttingInRepository.Query
                                     where cut.CuttingType == "Main Fabric" &&
                                     cut.CuttingInDate <= dateTo
                                     select new
                                     {
                                         cut.Identity,
                                         cut.FC,
                                         cut.RONo
                                     })
                          join b in GarmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in GarmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new
                          {
                              a.FC,
                              a.RONo,
                              FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC),
                              c.CuttingInQuantity
                          }).GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                          {
                              RO = key.RONo,
                              Total = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
                          });


            var QueryCuttingIn = from a in (from aa in GarmentSampleCuttingInRepository.Query
                                            where aa.UnitId == request.unit && aa.CuttingInDate <= dateTo && aa.CuttingType == "Main Fabric" 
                                            select new
                                            {
                                                aa.Identity,
                                                aa.RONo,
                                                aa.CuttingInDate,
                                                aa.Article
                                            })
                                 join b in GarmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                 join c in GarmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                 select new monitoringView
                                 {
                                     buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                     price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
                                     fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                     cuttingQtyMeter = 0,
                                     remainQty = 0,
                                     stock = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0,
                                     cuttingQtyPcs = a.CuttingInDate >= dateFrom ?
                                     c.CuttingInQuantity : 0,
                                     roJob = a.RONo,
                                     article = a.Article,
                                     style = (from buyer in GarmentSampleCuttingOutRepository.Query where buyer.RONo == a.RONo select buyer.ComodityName).FirstOrDefault(),
                                     expenditure = 0
                                 };


            var QueryCuttingOut = from a in (from aa in GarmentSampleCuttingOutRepository.Query
                                             where aa.UnitFromId == request.unit && aa.CuttingOutDate <= dateTo 
                                             select new { aa.Identity, aa.RONo, aa.Article, aa.CuttingOutDate, aa.ComodityName })
                                  join b in GarmentSampleCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
                                  select new monitoringView
                                  {
                                      buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                      price = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault(),
                                      fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                      cuttingQtyMeter = 0,
                                      remainQty = 0,
                                      stock = a.CuttingOutDate < dateFrom ? -b.TotalCuttingOut : 0,
                                      cuttingQtyPcs = 0,
                                      roJob = a.RONo,
                                      article = a.Article,
                                      style = a.ComodityName,
                                      expenditure = a.CuttingOutDate >= dateFrom ? b.TotalCuttingOut : 0
                                  };

            var QueryAvalComp = from a in (from aa in GarmentSampleAvalComponentRepository.Query
                                           where aa.UnitId == request.unit && aa.Date <= dateTo 
                                           select new { aa.Identity, aa.RONo, aa.Article, aa.Date, aa.ComodityName })
                                join b in GarmentSampleAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
                                select new monitoringView
                                {
                                    buyerCode = (from buyer in GarmentSamplePreparingRepository.Query where buyer.RONo == a.RONo select buyer.BuyerCode).FirstOrDefault(),
                                    price = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault(),
                                    fc = (from cost in sumFCs where cost.RO == a.RONo select cost.Total).FirstOrDefault(),
                                    cuttingQtyMeter = 0,
                                    remainQty = 0,
                                    stock = a.Date < dateFrom  ? -b.Quantity : 0,
                                    cuttingQtyPcs = 0,
                                    roJob = a.RONo,
                                    article = a.Article,
                                    style = a.ComodityName,
                                    expenditure = a.Date >= dateFrom ? b.Quantity : 0
                                };
            
            var queryNow = QueryCuttingIn.Union(QueryCuttingOut).Union(QueryAvalComp);
            var roList = (from a in queryNow
                          select a.roJob).Distinct().ToList();

            var sample = from s in GarmentSampleRequestRepository.Query
                             //join p in GarmentSampleRequestProductRepository.Query on s.Identity equals p.SampleRequestId
                         select new
                         {
                             s.RONoSample,
                             s.ComodityName,
                             s.BuyerCode,
                             Quantity = GarmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
                         };
            var queryReport = from a in queryNow
                              select new monitoringView
                              {
                                  buyerCode = (from s in sample where s.RONoSample == a.roJob select s.BuyerCode).FirstOrDefault(),
                                  price = a.price,
                                  fc = a.fc,
                                  cuttingQtyMeter = a.cuttingQtyMeter,
                                  remainQty = a.remainQty,
                                  stock = a.stock,
                                  cuttingQtyPcs = a.cuttingQtyPcs,
                                  roJob = a.roJob,
                                  article = a.article,
                                  style = (from s in sample where s.RONoSample == a.roJob select s.ComodityName).FirstOrDefault(),
                                  expenditure = a.expenditure,
                                 // hours = (from cost in costCalculation.data where cost.ro == a.roJob select cost.hours).FirstOrDefault(),
                                  qtyOrder = (from s in sample where s.RONoSample == a.roJob select s.Quantity).FirstOrDefault()
                              };
            var ccc = queryReport.ToList();
            foreach (var item in ccc)
            {
                item.fc = Math.Round(item.fc, 2);
            }

            var querySum = ccc.GroupBy(x => new
            {
                x.price,
                x.fc,
                x.buyerCode,
                x.qtyOrder,
                x.roJob,
                x.article,
                x.style,
                x.hours
            }, (key, group) => new
            {
                QtyOrder = key.qtyOrder,
                RoJob = key.roJob,
                Fc = key.fc,
                Stock = group.Sum(s => s.stock),
                buyer = key.buyerCode,
                bPrice = key.price,
                Article = key.article,
                Style = key.style,
                CuttingQtyPcs = group.Sum(s => s.cuttingQtyPcs),
                CuttingQtyMeter = group.Sum(s => s.cuttingQtyMeter),
                Expenditure = group.Sum(s => s.expenditure),
                Hours = key.hours
            }).OrderBy(s => s.RoJob);

            GarmentSampleCuttingMonitoringViewModel listViewModel = new GarmentSampleCuttingMonitoringViewModel();
            List<GarmentSampleCuttingMonitoringDto> monitoringCuttingDtos = new List<GarmentSampleCuttingMonitoringDto>();
            foreach (var item in querySum)
            {
                GarmentSampleCuttingMonitoringDto cuttingDto = new GarmentSampleCuttingMonitoringDto
                {
                    roJob = item.RoJob,
                    article = item.Article,
                    style = item.Style,
                    hours = item.Hours,
                    qtyOrder = item.QtyOrder,
                    cuttingQtyPcs = item.CuttingQtyPcs,
                    expenditure = item.Expenditure,
                    stock = item.Stock,
                    remainQty = item.Stock + item.CuttingQtyPcs - item.Expenditure,
                    fc = Math.Round(item.Fc, 2),
                    cuttingQtyMeter = Math.Round(item.Fc * item.CuttingQtyPcs, 2),
                    price = Math.Round(Convert.ToDecimal(item.bPrice), 2) * Convert.ToDecimal(Math.Round(item.Fc, 2)),
                    buyerCode = item.buyer

                };
                monitoringCuttingDtos.Add(cuttingDto);
            }
            var data = from a in monitoringCuttingDtos
                       where a.stock > 0 || a.expenditure > 0 || a.cuttingQtyPcs > 0 || a.remainQty > 0
                       select a;
            listViewModel.garmentMonitorings = data.ToList();
            return listViewModel;
        }
        class monitoringView
        {
            public string roJob { get; set; }
            public string article { get; set; }
            public string buyerCode { get; set; }
            public double qtyOrder { get; set; }
            public string style { get; set; }
            public double fc { get; set; }
            public double hours { get; set; }
            public double cuttingQtyMeter { get; set; }
            public double stock { get; set; }
            public double cuttingQtyPcs { get; set; }
            public double expenditure { get; set; }
            public double remainQty { get; set; }
            public decimal price { get; set; }
        }
    }
}
