using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using Manufactures.Domain.MonitoringProductionFlow;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using System.Net.Http;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionFlows.Queries
{
	public class GetMonitoringProductionFlowQueryHandler : IQueryHandler<GetMonitoringProductionFlowQuery, GarmentMonitoringProductionFlowListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSewingOutDetailRepository garmentSewingOutDetailRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingOutDetailRepository garmentFinishingOutDetailRepository;

		public GetMonitoringProductionFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}
		 
		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{

            CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

            var listRO = string.Join(",", ro.Distinct());
            var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

            var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

            var freeRO = new List<string>();

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

                foreach (var item in ro)
                {
                    var data = listData.SingleOrDefault(s => s.ro == item);
                    if (data != null)
                    {
                        costCalculationGarmentDataProductionReport.data.Add(data);
                    }
                    else
                    {
                        freeRO.Add(item);
                    }
                }
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();

            }
            return costCalculationGarmentDataProductionReport;
		}
		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public string Size { get; internal set; }
			public double QtyCutting { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double QtySewing { get; internal set; }
			public double QtyFinishing { get; internal set; }
			public double Wip { get; internal set; }

		}
		public async Task<GarmentMonitoringProductionFlowListViewModel> Handle(GetMonitoringProductionFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset date = new DateTimeOffset(request.date);
			date.AddHours(7);
			var QueryRo = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 where a.UnitFromId == request.unit && a.CuttingOutDate <= date 
									 select a.RONo).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			
			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
									 join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
									 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= date   
								   select new  monitoringView{ Ro = a.RONo,Article= a.Article, Comodity= a.ComodityName,BuyerCode= (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),QtyOrder= (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(),QtyCutting= c.CuttingOutQuantity,Size= c.SizeName });

			var QueryLoading = (from a in garmentLoadingRepository.Query
								   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
								   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.LoadingDate <= date
								   select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyLoading = b.Quantity, Size = b.SizeName });

			var QuerySewingOutIsDifSize = from a in garmentSewingOutRepository.Query
									join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
									join c in garmentSewingOutDetailRepository.Query on b.Identity equals c.SewingOutItemId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.SewingTo == "FINISHING" &&  a.UnitId == request.unit && a.SewingOutDate <= date
								    select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = c.Quantity, Size =c.SizeName };
			var QuerySewingOut = from a in garmentSewingOutRepository.Query
								   join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
								   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date && a.IsDifferentSize == false
								 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = b.Quantity, Size = b.SizeName };
			var QueryFinishingOutisDifSize = from a in garmentFinishingOutRepository.Query
								 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
								 join c in garmentFinishingOutDetailRepository.Query on b.Identity equals c.FinishingOutItemId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.FinishingTo== "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date
									select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = c.Quantity, Size = c.SizeName };
			var QueryFinishingOut = from a in garmentFinishingOutRepository.Query
									join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date && a.IsDifferentSize == false
									select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = b.Quantity, Size = b.SizeName };


			var queryNow = QueryCuttingOut.Union(QueryLoading).Union(QuerySewingOutIsDifSize).Union(QuerySewingOut).Union(QueryFinishingOut).Union(QueryFinishingOutisDifSize).AsEnumerable();

			var querySum = queryNow.GroupBy(x => new { x.Size,x.Ro, x.Article, x.BuyerCode, x.Comodity,x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				buyer= key.BuyerCode,
				comodity= key.Comodity,
				qtyOrder= key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCutting),
				qtySewing = group.Sum(s => s.QtySewing),
				qtyLoading= group.Sum(s => s.QtyLoading),
				qtyFinishing= group.Sum(s => s.QtyFinishing),
				size= key.Size,
			});
			var querySumTotal = queryNow.GroupBy(x => new {  x.Ro, x.Article, x.BuyerCode, x.Comodity, x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				buyer = key.BuyerCode,
				comodity = key.Comodity,
				qtyOrder = key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCutting),
				qtySewing = group.Sum(s => s.QtySewing),
				qtyLoading = group.Sum(s => s.QtyLoading),
				qtyFinishing = group.Sum(s => s.QtyFinishing),
				size="TOTAL"
			});

			var query = querySum.Union(querySumTotal).OrderBy(s => s.ro);
			GarmentMonitoringProductionFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionFlowListViewModel();
			List<GarmentMonitoringProductionFlowDto> monitoringDtos = new List<GarmentMonitoringProductionFlowDto>();

			foreach (var item in query)
			{
				GarmentMonitoringProductionFlowDto garmentMonitoringDto = new GarmentMonitoringProductionFlowDto()
				{
					Article = item.article,
					Ro = item.ro,
					BuyerCode = item.buyer,
					QtyOrder = item.qtyOrder,
					QtyCutting = item.qtycutting,
					QtySewing = item.qtySewing,
					QtyFinishing = item.qtyFinishing,
					QtyLoading = item.qtyLoading,
					Size = item.size,
					Comodity = item.comodity,
					Wip = item.qtycutting - item.qtyFinishing
				};
				monitoringDtos.Add(garmentMonitoringDto);
			}
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;

			return garmentMonitoringProductionFlow;
		}
	}
}
