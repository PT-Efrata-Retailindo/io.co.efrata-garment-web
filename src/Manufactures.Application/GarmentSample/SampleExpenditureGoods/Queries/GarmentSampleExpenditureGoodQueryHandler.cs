using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using System.Linq;
using System.Net.Http;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Threading;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries
{
    public class GarmentSampleExpenditureGoodQueryHandler : IQueryHandler<GetMonitoringSampleExpenditureGoodQuery, GarmentMonitoringSampleExpenditureGoodListViewModel>
    {
        private readonly IStorage _storage;
        protected readonly IHttpClientService _http;
        private readonly IGarmentSampleExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository GarmentSampleRequestProductRepository;
        public GarmentSampleExpenditureGoodQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _http = serviceProvider.GetService<IHttpClientService>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
        }

        class monitoringView
        {
            public string expenditureGoodNo { get; internal set; }
            public string expenditureGoodType { get; internal set; }
            public string buyerCode { get; internal set; }
            public DateTimeOffset expenditureDate { get; internal set; }
            public string roNo { get; internal set; }
            public string buyerArticle { get; internal set; }
            public string colour { get; internal set; }
            public string name { get; internal set; }
            public string unitname { get; internal set; }
            public double qty { get; internal set; }
            public string invoice { get; internal set; }
            public decimal price { get; internal set; }
            public double fc { get; internal set; }
        }

        public async Task<PEBResult> GetDataPEB(List<string> invoice, string token)
        {
            PEBResult pEB = new PEBResult();

            var listInvoice = string.Join(",", invoice.Distinct());
            var stringcontent = new StringContent(JsonConvert.SerializeObject(listInvoice), Encoding.UTF8, "application/json");

            var garmentProductionUri = CustomsDataSettings.Endpoint + $"customs-reports/getPEB";
            var httpResponse = await _http.SendAsync(HttpMethod.Get, garmentProductionUri, token, stringcontent);



            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                var listdata = JsonConvert.DeserializeObject<List<PEBResultViewModel>>(dataString);

                foreach (var i in listdata)
                {
                    pEB.data.Add(i);
                }

            }

            return pEB;
        }
        
        class ViewFC
        {
            public string RO { get; internal set; }
            public double FC { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgFC { get; set; }

        }
        class ViewBasicPrices
        {
            public string RO { get; internal set; }
            public decimal BasicPrice { get; internal set; }
            public int Count { get; internal set; }
            //Enhance Jason Aug 2021
            public double AvgBasicPrice { get; set; }

        }
        public async Task<GarmentMonitoringSampleExpenditureGoodListViewModel> Handle(GetMonitoringSampleExpenditureGoodQuery request, CancellationToken cancellationToken)
        {
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);


			var QueryRo = (from a in garmentExpenditureGoodRepository.Query
                           where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo
                           select a.RONo).Distinct();

            List<string> _ro = new List<string>();
            foreach (var item in QueryRo)
            {
                _ro.Add(item);
            }

            GarmentMonitoringSampleExpenditureGoodListViewModel listViewModel = new GarmentMonitoringSampleExpenditureGoodListViewModel();
            List<GarmentMonitoringSampleExpenditureGoodDto> monitoringDtos = new List<GarmentMonitoringSampleExpenditureGoodDto>();
            var sumbasicPrice = (from a in garmentPreparingRepository.Query
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                 where /*(request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&*/
                                 a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)
                                 select new { a.RONo, b.BasicPrice })
                        .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                        {
                            RO = key.RONo,
                            BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                            Count = group.Count(),
                            AvgBasicPrice = (double)(Convert.ToDecimal(group.Sum(s => s.BasicPrice)) / group.Count())
                        });

            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where  a.CuttingType == "Main Fabric" &&
                         a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.CuttingInDate <= dateTo
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
            var Query = from a in (from aa in garmentExpenditureGoodRepository.Query
                                   where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate >= dateFrom && aa.ExpenditureDate <= dateTo
                                   select aa)
                        join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                        where a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) && a.ExpenditureDate >= dateFrom && a.ExpenditureDate <= dateTo

                        select new monitoringView
                        {
                            fc = (from aa in sumFCs where aa.RO == a.RONo select aa.AvgFC).FirstOrDefault(),
                            price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.AvgBasicPrice).FirstOrDefault()),
                            buyerCode = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.BuyerCode).FirstOrDefault(),
                            buyerArticle = a.BuyerCode + " " + a.Article,
                            roNo = a.RONo,
                            expenditureDate = a.ExpenditureDate,
                            expenditureGoodNo = a.ExpenditureGoodNo,
                            expenditureGoodType = a.ExpenditureType,
                            invoice = a.Invoice,
                            colour = b.Description,
                            qty = b.Quantity,
                            name = (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.ComodityName).FirstOrDefault(),
                            unitname = a.UnitName
                        };

            var querySum = Query.ToList().GroupBy(x => new { x.fc, x.buyerCode, x.buyerArticle, x.roNo, x.expenditureDate, x.expenditureGoodNo, x.expenditureGoodType, x.invoice, x.colour, x.name, x.unitname }, (key, group) => new
            {
                ros = key.roNo,
                buyer = key.buyerArticle,
                expenditureDates = key.expenditureDate,
                qty = group.Sum(s => s.qty),
                expendituregoodNo = key.expenditureGoodNo,
                expendituregoodTypes = key.expenditureGoodType,
                color = key.colour,
                price = group.Sum(s => s.price),
                buyerC = key.buyerCode,
                names = key.name,
                unitname = key.unitname,
                invoices = key.invoice,
                fcs = key.fc

            }).OrderBy(s => s.expendituregoodNo);

            var Pebs = await GetDataPEB(querySum.Select(x => x.invoices).ToList(), request.token);

            foreach (var item in querySum)
            {
                var peb = Pebs.data.FirstOrDefault(x => x.BonNo.Trim() == item.invoices);

                GarmentMonitoringSampleExpenditureGoodDto dto = new GarmentMonitoringSampleExpenditureGoodDto
                {
                    roNo = item.ros,
                    buyerArticle = item.buyer,
                    expenditureGoodType = item.expendituregoodTypes,
                    pebDate = peb == null ? new DateTime(1970, 01, 01) : peb.BCDate,
                    expenditureGoodNo = item.expendituregoodNo,
                    expenditureDate = item.expenditureDates,
                    qty = item.qty,
                    colour = item.color,
                    name = item.names,
                    unitname = item.unitname,
                    invoice = item.invoices,
                    price = Math.Round(Convert.ToDecimal(Convert.ToDouble(Math.Round(item.price, 2)) * Math.Round(item.fcs, 2)), 2),
                    buyerCode = item.buyerC

                };
                monitoringDtos.Add(dto);
            }
            listViewModel.garmentMonitorings = monitoringDtos;
            return listViewModel;
        }
    }
}
