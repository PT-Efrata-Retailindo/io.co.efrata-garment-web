using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;

namespace Manufactures.Application.GarmentSample.SamplePreparings.Queries.GetMonitoringPrepareSample
{
	public class GetMonitoringSamplePrepareQueryHandler : IQueryHandler<GetMonitoringSamplePrepareQuery, GarmentMonitoringSamplePrepareViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
		private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentSampleAvalProductRepository garmentAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository garmentAvalProductItemRepository;
        private readonly IGarmentSampleDeliveryReturnRepository garmentDeliveryReturnRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;

        public GetMonitoringSamplePrepareQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            garmentAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            garmentAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            garmentDeliveryReturnRepository = storage.GetRepository<IGarmentSampleDeliveryReturnRepository>();
            garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
		}
		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string productCode { get; set; }
			public string remark { get; set; }
			public string uomUnit { get; set; }
			public string roAsal { get; set; }
			public double stock { get; set; }
			public double receipt { get; set; }
			public double mainFabricExpenditure { get; set; }
			public double nonMainFabricExpenditure { get; set; }
			public double expenditure { get; set; }
			public double aval { get; set; }
			public double remainQty { get; set; }
			
			public Guid prepareItemid { get; set; }
			public decimal price { get; set; }
		}
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal Total { get; internal set; }
		}

		public async Task<GarmentMonitoringSamplePrepareViewModel> Handle(GetMonitoringSamplePrepareQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
            dateTo = dateTo.AddHours(7);
			var sumbasicPrice = (from a in (from aa in garmentPreparingRepository.Query
											where aa.UnitId == request.unit
											select new
											{
												aa.Identity,
												aa.RONo
											})
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
								 select new
								 {
									 a.RONo,
									 b.BasicPrice
								 })
				   .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
				   {
					   RO = key.RONo,
					   Total = Convert.ToDecimal(group.Sum(s => s.BasicPrice) / group.Count())
						   //BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
						   //Count = group.Count()
					   });
			var QueryMutationPrepareNow = from a in (from aa in garmentPreparingRepository.Query
                                                     where aa.UnitId == request.unit && aa.ProcessDate <= dateTo
                                                     select new
                                                     {
                                                         aa.Identity,
                                                         aa.Article,
                                                         aa.BuyerCode,aa.BuyerName,
                                                         aa.RONo,
                                                         aa.ProcessDate
                                                     })
                                          join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                          select new
                                          {
                                              Buyer = a.BuyerCode +"-" + a.BuyerName,
                                              RO = a.RONo,
                                              Articles = a.Article,
                                              Id = a.Identity,
                                              DetailExpend = b.UENItemId,
                                              Processdate = a.ProcessDate
                                          };

          
            var QueryMutationPrepareItemsROASAL = (from a in QueryMutationPrepareNow
                                                   join b in garmentPreparingItemRepository.Query on a.Id equals b.GarmentSamplePreparingId
                                                   where b.UENItemId == a.DetailExpend
                                                   select new
                                                   {
                                                       article = a.Articles,
                                                       roJob = a.RO,
                                                       buyerCode = a.Buyer,
                                                       prepareitemid = b.Identity,
                                                       roasal = b.ROSource,
													   price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RO select aa.Total).FirstOrDefault())
												   });

          
            var QueryCuttingDONow = from a in (from data in garmentCuttingInRepository.Query
                                               where data.UnitId == request.unit && data.CuttingInDate <= dateTo
                                               select new
                                               {
                                                   data.RONo,
                                                   data.Identity,
                                                   data.CuttingInDate,
                                                   data.CuttingType
                                               })
                                    join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                    join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                    select new monitoringView
                                    {
                                        prepareItemid = c.PreparingItemId,
                                        expenditure = 0,
                                        aval = 0,
                                        uomUnit = "",
                                        stock = a.CuttingInDate < dateFrom ? -c.PreparingQuantity : 0,
                                        nonMainFabricExpenditure = a.CuttingType == "Non Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0,
                                        mainFabricExpenditure = a.CuttingType == "Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0,
                                        remark = c.DesignColor,
                                        receipt = 0,
                                        productCode = c.ProductCode,
                                        remainQty = 0,
										price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault())
									};

            var QueryMutationPrepareItemNow = (from d in QueryMutationPrepareNow
                                               join e in garmentPreparingItemRepository.Query on d.Id equals e.GarmentSamplePreparingId
                                               where e.UENItemId == d.DetailExpend
                                               select new monitoringView
                                               {
                                                   prepareItemid = e.Identity,
                                                   uomUnit = "",
                                                   stock = d.Processdate < dateFrom ? e.Quantity : 0,
                                                   mainFabricExpenditure = 0,
                                                   nonMainFabricExpenditure = 0,
                                                   remark = e.DesignColor,
                                                   receipt = (d.Processdate >= dateFrom ? e.Quantity : 0),
                                                   productCode = e.ProductCode,
                                                   remainQty = e.RemainingQuantity,
												   price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == d.RO select aa.Total).FirstOrDefault())

											   }).Distinct();

            var QueryAval = from a in (from data in garmentAvalProductRepository.Query
                                       where data.AvalDate <= dateTo
                                       select new
                                       {
                                           data.Identity,
                                           data.RONo,
                                           data.AvalDate
                                       })
                            join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
                            join c in garmentPreparingItemRepository.Query on Guid.Parse(b.SamplePreparingItemId) equals c.Identity
                            join d in (from data in garmentPreparingRepository.Query
                                       where data.UnitId == request.unit
                                       select new
                                       {
                                           data.Identity,
                                           data.RONo
                                       }) on c.GarmentSamplePreparingId equals d.Identity
                            select new monitoringView
                            {
                                prepareItemid = c.Identity,
                                //price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
                                expenditure = 0,
                                aval = a.AvalDate >= dateFrom ? b.Quantity : 0,
                                uomUnit = "",
                                stock = a.AvalDate < dateFrom ? -b.Quantity : 0,
                                mainFabricExpenditure = 0,
                                nonMainFabricExpenditure = 0,
                                remark = b.DesignColor,
                                receipt = 0,
                                productCode = b.ProductCode,
                                remainQty = 0,
								price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault())

							};

            var QueryDRPrepare = from a in (from data in garmentDeliveryReturnRepository.Query
                                            where data.ReturnDate <= dateTo && data.UnitId == request.unit
                                            && data.StorageName.Contains("BAHAN BAKU")
                                            select new
                                            {
                                                data.RONo,
                                                data.Identity,
                                                data.ReturnDate
                                            })
                                 join b in (from bb in garmentDeliveryReturnItemRepository.Query
                                            where bb.PreparingItemId != "00000000-0000-0000-0000-000000000000"
                                            select new
                                            {
                                                bb.PreparingItemId,
                                                bb.DRId,
                                                bb.Quantity,
                                                bb.ProductCode,
                                                bb.DesignColor
                                            }) on a.Identity equals b.DRId
                                 select new
                                 {
                                     a.RONo,
                                     b.PreparingItemId,
                                     b.Quantity,
                                     b.DesignColor,
                                     a.ReturnDate,
                                     b.ProductCode
                                 };

            var QueryDeliveryReturn = from a in QueryDRPrepare
                                      join c in garmentPreparingItemRepository.Query
                                      on Guid.Parse(a.PreparingItemId) equals (c.Identity)
                                      select new monitoringView
                                      {
                                          prepareItemid = c.Identity,
                                          expenditure = a.ReturnDate >= dateFrom ? a.Quantity : 0,
                                          aval = 0,
                                          uomUnit = "",
                                          stock = a.ReturnDate < dateFrom ? -a.Quantity : 0,
                                          mainFabricExpenditure = 0,
                                          nonMainFabricExpenditure = 0,
                                          remark = a.DesignColor,
                                          receipt = 0,
                                          productCode = a.ProductCode,
                                          remainQty = 0,
										  price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault())

									  };

            var queryNow = from a in (QueryMutationPrepareItemNow
                            .Union(QueryCuttingDONow)
                            .Union(QueryAval)
                            .Union(QueryDeliveryReturn)
                            .AsEnumerable())
                           join b in QueryMutationPrepareItemsROASAL
                           on a.prepareItemid equals b.prepareitemid
                           select new { a, b };


            var querySum = queryNow.GroupBy(x => new { x.b.price, x.b.roasal, x.b.roJob, x.b.article, x.b.buyerCode, x.a.productCode, x.a.remark }, (key, group) => new
            {
                ROAsal = key.roasal,
                ROJob = key.roJob,
                stock = group.Sum(s => s.a.stock),
                ProductCode = key.productCode,
                Article = key.article,
                buyer = key.buyerCode,
                Remark = key.remark,
				price = key.price,
                mainFabricExpenditure = group.Sum(s => s.a.mainFabricExpenditure),
                nonmainFabricExpenditure = group.Sum(s => s.a.nonMainFabricExpenditure),
                receipt = group.Sum(s => s.a.receipt),
                Aval = group.Sum(s => s.a.aval),
                drQty = group.Sum(s => s.a.expenditure)
            }).OrderBy(s => s.ROJob);


            GarmentMonitoringSamplePrepareViewModel garmentMonitoringPrepareViewModel = new GarmentMonitoringSamplePrepareViewModel();
            List<GarmentMonitoringSamplePrepareDto> monitoringPrepareDtos = new List<GarmentMonitoringSamplePrepareDto>();
            foreach (var item in querySum)
            {
                GarmentMonitoringSamplePrepareDto garmentMonitoringPrepareDto = new GarmentMonitoringSamplePrepareDto()
                {
                    article = item.Article,
                    roNo = item.ROJob,
                    productCode = item.ProductCode,
                    roSource = item.ROAsal,
                    uomUnit = "MT",
                    remainQty = Math.Round(item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty, 2),
                    stock = Math.Round(item.stock, 2),
                    remark = item.Remark,
                    receipt = Math.Round(item.receipt, 2),
                    deliveryReturn = Math.Round(item.drQty,2),
                    aval = Math.Round(item.Aval, 2),
                    nonMainFabricExpenditure = Math.Round(item.nonmainFabricExpenditure, 2),
                    mainFabricExpenditure = Math.Round(item.mainFabricExpenditure, 2),
					price = Math.Round(item.price, 2),
					nominal = (item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty) * Convert.ToDouble(item.price)
				};
                monitoringPrepareDtos.Add(garmentMonitoringPrepareDto);
            }
            var datas = from aa in monitoringPrepareDtos
						where Math.Round(aa.stock, 2) > 0 || Math.Round(aa.receipt, 2) > 0 || Math.Round(aa.aval, 2) > 0 || Math.Round(aa.mainFabricExpenditure, 2) > 0 || Math.Round(aa.nonMainFabricExpenditure, 2) > 0 || Math.Round(aa.remainQty, 2) > 0
						select aa;
			monitoringPrepareDtos = datas.ToList();
			garmentMonitoringPrepareViewModel.garmentMonitorings = monitoringPrepareDtos;

			return garmentMonitoringPrepareViewModel;
		}
	}
}
