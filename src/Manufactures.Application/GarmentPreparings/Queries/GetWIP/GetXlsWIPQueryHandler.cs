using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using OfficeOpenXml;
using System.Net.Http;

namespace Manufactures.Application.GarmentPreparings.Queries.GetWIP
{
    public class GetXlsWIPQueryHandler : IQueryHandler<GetXlsWIPQuery, MemoryStream>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
        private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
        private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;



        public GetXlsWIPQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();


            _http = serviceProvider.GetService<IHttpClientService>();
        }

        class monitoringViewTemp
        {
            public string itemCode { get; internal set; }
            public string unitQty { get; internal set; }
            public double Quantity { get; internal set; }
        }

        class monitoringViewsTemp
        {
            public string itemname { get; internal set; }
            public string itemCode { get; internal set; }
            public string unitQty { get; internal set; }
            public double Quantity { get; internal set; }
        }

        public async Task<GarmentProductResult> GetProducts(string codes, string token)
        {
            GarmentProductResult garmentProduct = new GarmentProductResult();


            var garmentProductionUri = MasterDataSettings.Endpoint + $"master/garmentProducts/byCode?code={codes}";

            var httpContent = new StringContent(JsonConvert.SerializeObject(codes), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get,garmentProductionUri, token, httpContent);



            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();

                if (dataString != null)
                {
                    var listdata = JsonConvert.DeserializeObject<List<GarmentProductViewModel>>(dataString);

                    foreach (var i in listdata)
                    {
                        garmentProduct.data.Add(i);
                    }
                    //garmentProduct.data = listdata;
                }


            }

            return garmentProduct;



        }

        public async Task<MemoryStream> Handle(GetXlsWIPQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset dateFrom = new DateTimeOffset(request.Date.AddHours(7));
            GarmentWIPListViewModel listViewModel = new GarmentWIPListViewModel();
            List<GarmentWIPDto> monitoringDtos = new List<GarmentWIPDto>();
            var FactPreparePreparing = from a in (from aa in garmentPreparingRepository.Query
                                                  //where aa.ProcessDate.Value.Date < request.Date.Date
                                                  where aa.ProcessDate.Value.Year == request.Date.Year
                                                  select aa)
                                       join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                                       select new monitoringViewTemp
                                       {
                                           itemCode = b.ProductCode,
                                           unitQty = b.UomUnit,
                                           Quantity = b.Quantity
                                       };

            var FactPrepareCutting = from a in (from aa in garmentCuttingInRepository.Query
                                                //where aa.CuttingInDate.Date < request.Date.Date
                                                where aa.CuttingInDate.Year == request.Date.Year
                                                select aa)
                                     join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                     join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                     select new monitoringViewTemp
                                     {
                                         itemCode = c.ProductCode,
                                         unitQty = c.CuttingInUomUnit,
                                         Quantity = c.CuttingInQuantity * -1
                                     };
            var FactPrepareAvalProduct = from a in (from aa in garmentAvalProductRepository.Query
                                                    //where aa.AvalDate.Value.Date < request.Date.Date
                                                    where aa.AvalDate.Value.Year == request.Date.Year
                                                    select aa)
                                         join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
                                         select new monitoringViewTemp
                                         {
                                             itemCode = b.ProductCode,
                                             unitQty = b.UomUnit,
                                             Quantity = b.Quantity * -1
                                         };

            var FactPrepareDeliveryReturn = from a in (from aa in garmentPreparingRepository.Query
                                                       //where aa.ProcessDate.Value.Date < request.Date.Date
                                                       where aa.ProcessDate.Value.Year == request.Date.Year
                                                       select aa)
                                            join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                                            join c in garmentDeliveryReturnItemRepository.Query on b.Identity.ToString() equals c.PreparingItemId
                                            select new monitoringViewTemp
                                            {
                                                itemCode = c.ProductCode,
                                                unitQty = c.UomUnit,
                                                Quantity = c.Quantity * -1
                                            };

            var FactPrepareTemp = FactPreparePreparing.Concat(FactPrepareCutting).Concat(FactPrepareAvalProduct).Concat(FactPrepareDeliveryReturn).ToList();
            var FactPrepareTemp2 = FactPrepareTemp.GroupBy(x => new { x.itemCode, x.unitQty }, (key, groupdata) => new monitoringViewTemp
            {
                itemCode = key.itemCode,
                unitQty = key.unitQty,
                Quantity = groupdata.Sum(x => x.Quantity)
            }).ToList();

            List<monitoringViewsTemp> FactPrepare = new List<monitoringViewsTemp>();

            FactPrepareTemp2 = FactPrepareTemp2.Where(x => x.Quantity > 0.01).Select(x => x).ToList();



            //var pages = (int)Math.Ceiling((decimal)FactPrepareTemp2.Count() / (decimal)800);

            List<GarmentProductViewModel> GarmentProducts = new List<GarmentProductViewModel>();

            var code1 = string.Join(",", FactPrepareTemp2.Select(x => x.itemCode).ToList());
            GarmentProductResult GarmentProduct1 = await GetProducts(code1, request.token);

            foreach (var a in GarmentProduct1.data)
            {
                GarmentProducts.Add(a);
            }

            //for (int i = 1; i <= pages + 1; i++)
            //{
            //    var code1 = string.Join(",", FactPrepareTemp2.Skip((i - 1) * 800).Take(800).Select(x => x.itemCode).ToList());
            //    GarmentProductResult GarmentProduct1 = await GetProducts(code1, request.token);

            //    foreach (var a in GarmentProduct1.data)
            //    {
            //        GarmentProducts.Add(a);
            //    }

            //}

            foreach (var a in FactPrepareTemp2.Where(x => x.Quantity > 0.01))
            {

                var GarmentProduct = GarmentProducts.FirstOrDefault(x => x.Code == a.itemCode);

                var Composition = GarmentProduct == null ? "-" : GarmentProduct.Composition;
                var Width = GarmentProduct == null ? "-" : GarmentProduct.Width;
                var Const = GarmentProduct == null ? "-" : GarmentProduct.Const;
                var Yarn = GarmentProduct == null ? "-" : GarmentProduct.Yarn;


                FactPrepare.Add(new monitoringViewsTemp
                {
                    itemCode = a.itemCode,
                    itemname = string.Concat(Composition, "", Width, "", Const, "", Yarn),
                    Quantity = a.Quantity,
                    unitQty = a.unitQty
                });
            }

            var FactCutting = (from a in (from aa in garmentCuttingOutRepository.Query
                                          //where aa.CuttingOutDate.Date < request.Date.Date
                                          where aa.CuttingOutDate.Year == request.Date.Year
                                          select aa)
                               join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                               join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                               select new
                               {
                                   ComodityCode = a.ComodityCode,
                                   ComodityName = a.ComodityName,
                                   Quantity = c.CuttingOutQuantity
                               }).GroupBy(x => new { x.ComodityCode, x.ComodityName }, (key, group) => new monitoringViewsTemp
                               {
                                   itemname = key.ComodityName,
                                   itemCode = key.ComodityCode,
                                   Quantity = group.Sum(x => x.Quantity),
                                   unitQty = "PCS"
                               });

            var FactFinishing = (from a in (from aa in garmentFinishingOutRepository.Query
                                            //where aa.FinishingOutDate.Date < request.Date.Date
                                            where aa.FinishingOutDate.Year == request.Date.Year
                                            && aa.FinishingTo == "GUDANG JADI"
                                            select aa)
                                 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                 select new
                                 {
                                     ComodityCode = a.ComodityCode,
                                     ComodityName = a.ComodityName,
                                     Quantity = b.Quantity * -1,
                                 }).GroupBy(x => new { x.ComodityCode, x.ComodityName }, (key, group) => new monitoringViewsTemp
                                 {
                                     itemname = key.ComodityName,
                                     itemCode = key.ComodityCode,
                                     Quantity = group.Sum(x => x.Quantity),
                                     unitQty = "PCS"
                                 });

            var WIPTemp = FactCutting.Concat(FactFinishing).ToList();

            WIPTemp = WIPTemp.GroupBy(x => new { x.itemCode, x.itemname, x.unitQty }, (key, group) => new monitoringViewsTemp
            {
                itemname = key.itemname,
                itemCode = key.itemCode,
                Quantity = group.Sum(x => x.Quantity),
                unitQty = key.unitQty

            }).ToList();

            var WIP = WIPTemp.Where(x => x.Quantity > 0).Concat(FactPrepare).ToList();


            foreach (var i in WIP)
            {
                GarmentWIPDto dto = new GarmentWIPDto
                {
                    Kode = i.itemCode,
                    Comodity = i.itemname,
                    UnitQtyName = i.unitQty,
                    WIP = Math.Round(i.Quantity, 2)
                };

                monitoringDtos.Add(dto);
            }

            listViewModel.garmentWIP = monitoringDtos;

            var reportDataTable = new DataTable();
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NO", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "KODE BARANG", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "NAMA BARANG", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SATUAN", DataType = typeof(string) });
            reportDataTable.Columns.Add(new DataColumn() { ColumnName = "WIP", DataType = typeof(double) });

            int counter = 1;
            if (listViewModel.garmentWIP.Count > 0)
            {
                foreach (var report in listViewModel.garmentWIP)
                {
                    reportDataTable.Rows.Add(counter ,report.Kode, report.Comodity, report.UnitQtyName, report.WIP);
                    counter++;
                }
            }

            using (var package = new ExcelPackage()) {
                var worksheet = package.Workbook.Worksheets.Add("Sheet 1");

                worksheet.Cells["A2"].LoadFromDataTable(reportDataTable, true, OfficeOpenXml.Table.TableStyles.Light16);

                var stream = new MemoryStream();

                package.SaveAs(stream);

                return stream;

            }

            //List<int> detailExpendId = new List<int>();

        }
    }
}
