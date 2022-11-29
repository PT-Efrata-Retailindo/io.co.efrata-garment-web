using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Domain.Queries;
using System.Threading;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.MonitoringSewing
{
    public class GetMonitoringSampleSewingQueryHandler : IQueryHandler<GetMonitoringSampleSewingQuery, GarmentMonitoringSampleSewingListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository garmentSampleSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSampleSewingOutItemRepository;

		private readonly IGarmentSamplePreparingRepository garmentSamplePreparingRepository;
		private readonly IGarmentSamplePreparingItemRepository garmentSamplePreparingItemRepository;
		private readonly IGarmentSampleCuttingInRepository garmentSampleCuttingInRepository;
        private readonly IGarmentSampleSewingInRepository garmentSampleSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository garmentSampleSewingInItemRepository;
        //private readonly IGarmentSampleAdjustmentRepository GarmentSampleAdjustmentRepository;
        //private readonly IGarmentSampleAdjustmentItemRepository GarmentSampleAdjustmentItemRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;
        public GetMonitoringSampleSewingQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSampleSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
			garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
			garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
			garmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentSampleSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            garmentSampleSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            //GarmentSampleAdjustmentRepository = storage.GetRepository<IGarmentSampleAdjustmentRepository>();
            //GarmentSampleAdjustmentItemRepository = storage.GetRepository<IGarmentSampleAdjustmentItemRepository>();
            garmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
        }


       

        class monitoringView
        {
            public string roJob { get; internal set; }
            public string article { get; internal set; }
            public string buyerCode { get; internal set; }
            public double qtyOrder { get; internal set; }
            public double BeginingBalanceSewingQty { get; internal set; }
            public double QtySewingIn { get; internal set; }
            public double QtySewingOut { get; internal set; }
            public double QtySewingInTransfer { get; internal set; }
            public double WipSewingOut { get; internal set; }
            public double WipFinishingOut { get; internal set; }
            public double QtySewingRetur { get; internal set; }
            public double QtySewingAdj { get; internal set; }
            public double PriceSewingAdj { get; internal set; }
            public double EndBalanceSewingQty { get; internal set; }
            public double EndBalanceSewingPrice { get; internal set; }
            public decimal price { get; internal set; }
        }
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
        }
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public int Count { get; internal set; }
        }
        public async Task<GarmentMonitoringSampleSewingListViewModel> Handle(GetMonitoringSampleSewingQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);

			var sumbasicPrice = (from a in (from prep in garmentSamplePreparingRepository.Query
											select new { prep.RONo, prep.Identity })
								 join b in garmentSamplePreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId

								 select new { a.RONo, b.BasicPrice })
				  .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
				  {
					  RO = key.RONo,
					  BasicPrice = Math.Round(Convert.ToDecimal(group.Sum(s => s.BasicPrice)), 2),
					  Count = group.Count()
				  });

			var sumFCs = (from a in garmentSampleCuttingInRepository.Query
                          where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && */ a.CuttingType == "Main Fabric" //&&
                                                                                                                                                                /*a.UnitId == request.unit && a.CuttingInDate <= dateTo*/
                          join b in garmentSampleCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentSampleCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
                       .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                       {
                           RO = key.RONo,
                           FC = group.Sum(s => (s.FCs)),
                           Count = group.Sum(s => s.CuttingInQuantity)
                       });

            var QuerySewingOut = from a in (from aa in garmentSampleSewingOutRepository.Query
                                            where  aa.SewingOutDate <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
                                 join b in garmentSampleSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId

                                 select new monitoringView
                                 {
                                     price = 0,
                                     roJob = a.RONo,
                                     BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom  && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom  && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom  && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom  && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom &&  a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0),
                                     QtySewingIn = 0,
                                     QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
                                     WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
                                     QtySewingAdj = 0
                                     
                                 };

            var QuerySewingIn = from a in (from aa in garmentSampleSewingInRepository.Query
                                           where  aa.UnitId == request.unit && aa.SewingInDate <= dateTo
                                           select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
                                join b in garmentSampleSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                select new monitoringView
                                {
                                    price = 0,
                                    roJob = a.RONo,
                                    BeginingBalanceSewingQty = (a.SewingInDate < dateFrom &&  a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                    QtySewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                    QtySewingOut = 0,
                                    QtySewingInTransfer = 0,
                                    WipSewingOut = 0,
                                    WipFinishingOut = 0,
                                    QtySewingRetur = 0,
                                    QtySewingAdj = 0
                                };
            //var QuerySewingAdj = from a in (from aa in garmentAdjustmentRepository.Query
            //                                where aa.AdjustmentDate >= dateBalance && aa.UnitId == request.unit && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "SEWING"
            //                                select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
            //                     join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
            //                     select new monitoringView
            //                     {
            //                         price = 0,
            //                         roJob = a.RONo,
            //                         BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
            //                         QtySewingIn = 0,
            //                         QtySewingOut = 0,
            //                         QtySewingInTransfer = 0,
            //                         WipSewingOut = 0,
            //                         WipFinishingOut = 0,
            //                         QtySewingRetur = 0,
            //                         QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
            //                     };
            //var queryNow = queryBalanceSewing.Union(QuerySewingOut).Union(QuerySewingIn).Union(QuerySewingAdj);
            var queryNow = QuerySewingOut.Union(QuerySewingIn);
            var querySum = queryNow.ToList().GroupBy(x => new { x.roJob }, (key, group) => new
            {

                RoJob = key.roJob,
                BeginingBalanceSewingQty = group.Sum(s => s.BeginingBalanceSewingQty),
                QtySewingIn = group.Sum(s => s.QtySewingIn),
                QtySewingInTransfer = group.Sum(s => s.QtySewingInTransfer),
                QtySewingOut = group.Sum(s => s.QtySewingOut),
                QtySewingAdj = group.Sum(s => s.QtySewingAdj),
                WipFinishingOut = group.Sum(s => s.WipFinishingOut),
                WipSewingOut = group.Sum(s => s.WipSewingOut),
                QtySewingRetur = group.Sum(s => s.QtySewingRetur),


            }).OrderBy(s => s.RoJob);
            GarmentMonitoringSampleSewingListViewModel listViewModel = new GarmentMonitoringSampleSewingListViewModel();
            List<GarmentMonitoringSampleSewingDto> monitoringDtos = new List<GarmentMonitoringSampleSewingDto>();
            var sample = from s in garmentSampleRequestRepository.Query
                           select new
                         {
                             s.RONoSample,
                             s.ComodityName,
                             s.BuyerCode,
                             Quantity = garmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
                         };
            foreach (var item in querySum)
            {
                GarmentMonitoringSampleSewingDto dto = new GarmentMonitoringSampleSewingDto
                {
                    roJob = item.RoJob,
                    article= (from aa in garmentSampleSewingInRepository.Query where aa.RONo == item.RoJob select aa.Article).FirstOrDefault(),
                    buyerCode = (from aa in sample where aa.RONoSample == item.RoJob select aa.BuyerCode).FirstOrDefault(),
                    qtyOrder=(from aa in sample where aa.RONoSample == item.RoJob select aa.Quantity).FirstOrDefault(),
                    style= (from aa in sample where aa.RONoSample == item.RoJob select aa.ComodityName).FirstOrDefault(),
                    beginingBalanceSewingQty = item.BeginingBalanceSewingQty,
                    qtySewingIn = item.QtySewingIn,
                    qtySewingInTransfer = item.QtySewingInTransfer,
                    qtySewingOut = item.QtySewingOut,
                    wipSewingOut = item.WipSewingOut,
                    wipFinishingOut = item.WipFinishingOut,
                    qtySewingRetur = item.QtySewingRetur,
                    qtySewingAdj = item.QtySewingAdj,
                    endBalanceSewingQty = Math.Round(item.BeginingBalanceSewingQty + item.QtySewingIn - item.QtySewingOut + item.QtySewingInTransfer - item.WipSewingOut - item.WipFinishingOut - item.QtySewingRetur - item.QtySewingAdj, 2)
                    
                };
                monitoringDtos.Add(dto);
            }
            
            listViewModel.garmentMonitorings = monitoringDtos;
            var data = from a in monitoringDtos
                       where a.beginingBalanceSewingQty > 0 || a.qtySewingIn > 0 || a.qtySewingInTransfer > 0 || a.qtySewingOut > 0 || a.qtySewingRetur > 0 || a.qtySewingAdj > 0 || a.wipSewingOut > 0 || a.wipFinishingOut > 0
                       select a;
           
			foreach (var garment in data)
			{
				garment.price = Math.Round(Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDecimal((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.endBalanceSewingPrice = Math.Round(garment.endBalanceSewingQty * (double)garment.price, 2);
			}

			listViewModel.garmentMonitorings = data.ToList();
            return listViewModel;
        }

    }
}

