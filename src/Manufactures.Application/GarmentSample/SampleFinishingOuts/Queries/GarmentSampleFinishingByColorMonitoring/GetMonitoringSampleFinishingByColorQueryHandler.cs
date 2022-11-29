using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries.GarmentSampleFinishingByColorMonitoring
{
    public class GetMonitoringSampleFinishingByColorQueryHandler : IQueryHandler<GetSampleFinishingByColorMonitoringQuery, GarmentSampleFinishingByColorMonitoringListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository; 
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;

        public GetMonitoringSampleFinishingByColorQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
             
            _http = serviceProvider.GetService<IHttpClientService>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }

        class monitoringView
        {
            public string roJob { get; internal set; }
            public string article { get; internal set; }
            public string buyerCode { get; internal set; }
            public double qtyOrder { get; internal set; }
            public double stock { get; internal set; }
            public string style { get; internal set; }
			public string color { get; internal set; }
			public double sewingQtyPcs { get; internal set; }
            public double finishingQtyPcs { get; internal set; }
            public string uomUnit { get; internal set; }
            public double remainQty { get; internal set; }
            public decimal price { get; internal set; }
        }
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgBasicPrice { get; set; }
        }
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgFC { get; set; }
        }
        public async Task<GarmentSampleFinishingByColorMonitoringListViewModel> Handle(GetSampleFinishingByColorMonitoringQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);


			var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
											select new { prep.RONo, prep.Identity, prep.UnitId })
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId

								 select new { a.RONo, b.BasicPrice })
						.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
						{
							RO = key.RONo,
							BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
							Count = group.Count()

						});

			var sumFCs = (from a in garmentCuttingInRepository.Query
						  where a.CuttingType == "Main Fabric" &&
						  a.CuttingInDate <= dateTo
						  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
						  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
						  select new { a.FC, a.RONo, FCs = Convert.ToDouble(c.CuttingInQuantity * a.FC), c.CuttingInQuantity })
					   .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
					   {
						   RO = key.RONo,
						   FC = group.Sum(s => (s.FCs)),
						   Count = group.Sum(s => s.CuttingInQuantity),
						   AvgFC = group.Sum(s => (s.FCs)) / group.Sum(s => s.CuttingInQuantity)
					   });
			GarmentSampleFinishingByColorMonitoringListViewModel listViewModel = new GarmentSampleFinishingByColorMonitoringListViewModel();
			List<GarmentSampleFinishingByColorMonitoringDto> monitoringDtos = new List<GarmentSampleFinishingByColorMonitoringDto>();

			var QueryFinishing = from a in (from aa in garmentFinishingOutRepository.Query
											where aa.UnitId == request.unit && aa.FinishingOutDate <= dateTo
											select new { aa.Identity, aa.FinishingOutDate, aa.RONo, aa.Article })
								 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
								 select new monitoringView { color = b.Color,price = 0, finishingQtyPcs = a.FinishingOutDate >= dateFrom ? b.Quantity : 0, sewingQtyPcs = 0, uomUnit = "PCS", remainQty = 0, stock = a.FinishingOutDate < dateFrom ? -b.Quantity : 0, roJob = a.RONo, article = a.Article };
			var QuerySewingOut = from a in (from aa in garmentSewingOutRepository.Query
											where aa.UnitId == request.unit && aa.SewingOutDate <= dateTo && aa.SewingTo == "FINISHING"

											select new { aa.Identity, aa.SewingOutDate, aa.RONo, aa.Article })
								 join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
								 select new monitoringView { color = b.Color, price = 0, finishingQtyPcs = 0, sewingQtyPcs = a.SewingOutDate >= dateFrom ? b.Quantity : 0, uomUnit = "PCS", remainQty = 0, stock = a.SewingOutDate < dateFrom ? b.Quantity : 0, roJob = a.RONo, article = a.Article };
			var queryNow = QuerySewingOut.Union(QueryFinishing);
			var querySum = queryNow.ToList().GroupBy(x => new { x.roJob, x.article, x.uomUnit ,x.color}, (key, group) => new
			{

				RoJob = key.roJob,
				color = key.color,
				Stock = group.Sum(s => s.stock),
				UomUnit = key.uomUnit,
				Article = key.article,
				SewingQtyPcs = group.Sum(s => s.sewingQtyPcs),
				Finishing = group.Sum(s => s.finishingQtyPcs)
			}).OrderBy(s => s.RoJob);

			var querySumTotal = from p in querySum
					group p by 1 into g
			select new GarmentSampleFinishingByColorMonitoringDto
			{
				color ="",
				roJob = "TOTAL",
				uomUnit = "",
				article = "",
				stock = g.Sum(s => s.Stock),
				sewingOutQtyPcs = g.Sum(s => s.SewingQtyPcs),
				finishingOutQtyPcs = g.Sum(s => s.Finishing)
			};
			 
			foreach (var item in querySum)
			{
				GarmentSampleFinishingByColorMonitoringDto dto = new GarmentSampleFinishingByColorMonitoringDto
				{
					roJob = item.RoJob,
					article = item.Article,
					uomUnit = item.UomUnit,
					color= item.color,
					sewingOutQtyPcs = item.SewingQtyPcs,
					finishingOutQtyPcs = item.Finishing,
					stock = item.Stock,
					remainQty = item.Stock + item.SewingQtyPcs - item.Finishing
				};
				monitoringDtos.Add(dto);
			}
			foreach (var item in querySumTotal)
			{
				GarmentSampleFinishingByColorMonitoringDto dto = new GarmentSampleFinishingByColorMonitoringDto
				{
					roJob = item.roJob,
					article = item.article,
					uomUnit = item.uomUnit,
					color = item.color,
					sewingOutQtyPcs = item.sewingOutQtyPcs,
					finishingOutQtyPcs = item.finishingOutQtyPcs,
					stock = item.stock,
					remainQty = item.stock + item.sewingOutQtyPcs - item.finishingOutQtyPcs
				};
				monitoringDtos.Add(dto);
			}

			listViewModel.garmentMonitorings = monitoringDtos;
			var data = from a in monitoringDtos
					   where a.stock > 0 || a.sewingOutQtyPcs > 0 || a.finishingOutQtyPcs > 0 || a.remainQty > 0
					   select a;
			var roList = (from a in data
						  select a.roJob).Distinct().ToList();

			var sample = from s in GarmentSampleRequestRepository.Query
						 select new
						 {
							 s.RONoSample,
							 s.ComodityName,
							 s.BuyerCode,
							 Quantity = GarmentSampleRequestProductRepository.Query.Where(p => s.Identity == p.SampleRequestId).Sum(a => a.Quantity)
						 };

			foreach (var garment in data)
            {
				garment.buyerCode = garment.buyerCode == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.BuyerCode).FirstOrDefault() : garment.buyerCode;
				garment.style = garment.style == null ? (from sr in sample where sr.RONoSample == garment.roJob select sr.ComodityName).FirstOrDefault() : garment.style;
				garment.price = Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.roJob select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.roJob select cost.FC / cost.Count).FirstOrDefault());
				garment.nominal = Math.Round((Convert.ToDouble(garment.stock + garment.sewingOutQtyPcs - garment.finishingOutQtyPcs)) * garment.price, 2);
				garment.qtyOrder = (from sr in sample where sr.RONoSample == garment.roJob select sr.Quantity).FirstOrDefault();

			}
			listViewModel.garmentMonitorings = data.ToList();

            return listViewModel;
        }
    }
}
