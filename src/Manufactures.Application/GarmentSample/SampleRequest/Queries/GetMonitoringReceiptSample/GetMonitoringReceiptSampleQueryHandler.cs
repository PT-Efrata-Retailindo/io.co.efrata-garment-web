using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using System.Threading.Tasks;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentSectionResult;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using System.Threading;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample
{
    public class GetMonitoringReceiptSampleQueryHandler : IQueryHandler<GetMonitoringReceiptSampleQuery, GarmentMonitoringReceiptSampleViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;

        public GetMonitoringReceiptSampleQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _http = serviceProvider.GetService<IHttpClientService>();
        }
        public async Task<GarmentSectionResult> GetSectionNameById(List<int> id, string token)
        {
            List<GarmentSectionViewModel> sectionViewModels = new List<GarmentSectionViewModel>();

            GarmentSectionResult sectionResult = new GarmentSectionResult();
            foreach (var item in id.Distinct())
            {
                var uri =  MasterDataSettings.Endpoint + $"master/garment-sections/{item}";
                var httpResponse =  await _http.GetAsync(uri, token);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var a = httpResponse.Content.ReadAsStringAsync().Result;
                    Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
                    var b = keyValues.GetValueOrDefault("data").ToString();

                    var garmentSection = JsonConvert.DeserializeObject<GarmentSectionViewModel>(b);
                    GarmentSectionViewModel garmentSectionViewModel = new GarmentSectionViewModel
                    {
                        Id = garmentSection.Id,
                        Name = garmentSection.Name
                    };
                    sectionViewModels.Add(garmentSectionViewModel);
                }

            }
            sectionResult.data = sectionViewModels;
            return sectionResult;
        }
        class monitoringView
        {
            public string sampleRequestNo { get; set; }
            public string roNoSample { get; set; }
            public string sampleCategory { get; set; }
            public string sampleType { get; set; }
            public string buyer { get; set; }
            public string style { get; set; }
            public string color { get; set; }
            public string sizeName { get; set; }
            public string sizeDescription { get; set; }
            public double quantity { get; set; }
            public DateTimeOffset sentDate { get; set; }
            public DateTimeOffset receivedDate { get; set; }
            public string garmentSectionName { get; set; }
            public DateTimeOffset sampleRequestDate { get; set; }
        }
        public async Task<GarmentMonitoringReceiptSampleViewModel> Handle(GetMonitoringReceiptSampleQuery request, CancellationToken cancellationToken)
        {
            DateTimeOffset receivedDateFrom = new DateTimeOffset(request.receivedDateFrom);
            receivedDateFrom.AddHours(7);
            DateTimeOffset receivedDateTo = new DateTimeOffset(request.receivedDateTo);
            receivedDateTo = receivedDateTo.AddHours(7);

            var QuerySampleRequest = from a in (from aa in garmentSampleRequestRepository.Query
                                                where aa.ReceivedDate >= receivedDateFrom && aa.ReceivedDate <= receivedDateTo
                                                && aa.IsReceived== true && aa.IsRejected== false && aa.IsRevised == false
                                                select new
                                                {
                                                    aa.Identity,
                                                    aa.RONoSample,
                                                    aa.SampleCategory,
                                                    aa.SampleRequestNo,
                                                    aa.SampleType,
                                                    aa.BuyerCode,
                                                    aa.BuyerName,
                                                    aa.SentDate,
                                                    aa.ReceivedDate,
                                                    aa.Date,
                                                    aa.SectionId,
                                                    aa.SampleTo
                                                })
                                     join b in garmentSampleRequestProductRepository.Query on a.Identity equals b.SampleRequestId
                                     select new
                                     {
                                         Style = b.Style,
                                         Color = b.Color,
                                         SizeName = b.SizeName,
                                         SizeDescription = b.SizeDescription,
                                         Quantity = b.Quantity,
                                         RoNoSample = a.RONoSample,
                                         SampleCategory = a.SampleCategory,
                                         SampleRequestNo = a.SampleRequestNo,
                                         SampleType = a.SampleType,
                                         BuyerCode = a.BuyerCode,
                                         BuyerName = a.BuyerName,
                                         SentDate = a.SentDate,
                                         ReceivedDate = a.ReceivedDate,
                                         SampleRequestDate = a.Date,
                                         SectionId = a.SectionId,
                                         a.SampleTo
                                     };
            List<int> _sectionId = new List<int>();
            foreach (var item in QuerySampleRequest)
            {
                _sectionId.Add(item.SectionId);

            }
            _sectionId.Distinct();
            GarmentSectionResult garmentSectionResult = await GetSectionNameById(_sectionId, request.token);
            
            GarmentMonitoringReceiptSampleViewModel sampleViewModel = new GarmentMonitoringReceiptSampleViewModel();
            List<GarmentMonitoringReceiptSampleDto> sampleDtosList = new List<GarmentMonitoringReceiptSampleDto>();
            var queryGrouping= from a in QuerySampleRequest
                               group new
                               {
                                   SampleRequestNo = a.SampleRequestNo,
                                   RoNoSample = a.RoNoSample,
                                   SampleCategory = a.SampleCategory,
                                   SampleType= a.SampleType,
                                   BuyerCode=a.BuyerCode,
                                   BuyerName= a.BuyerName,
                                   SentDate= a.SentDate,
                                   ReceivedDate = a.ReceivedDate,
                                   SectionId = a.SectionId,
                                   SampleRequestDate = a.SampleRequestDate
                               }
                                by new
                                {
                                    Style = a.Style,
                                    Color = a.Color,
                                    SizeName = a.SizeName,
                                    SizeDescription = a.SizeDescription,
                                    Quantity = a.Quantity
                                    //Here you add other keys you want
                                } into groups

                               select new { grouping = groups.Key.Style,groups.Key.Style,groups.Key.Color, groups.Key.SizeName, groups.Key.SizeDescription, groups.Key.Quantity, Items = groups.ToList() };
            //int index = 0;
            //foreach (var item in queryGrouping)
            // {
            //    GarmentMonitoringReceiptSampleDto receiptSampleDto = new GarmentMonitoringReceiptSampleDto()
            //    {
            //        buyer =  item.Items[index].BuyerCode + " - " + item.Items[index].BuyerName,
            //        color = item.Color,
            //        quantity = item.Quantity,
            //        receivedDate = item.Items[index].ReceivedDate,
            //        roNoSample = item.Items[index].RoNoSample,
            //        sampleCategory = item.Items[index].SampleCategory,
            //        sampleRequestDate = item.Items[index].SampleRequestDate,
            //        sampleRequestNo = item.Items[index].SampleRequestNo,
            //        sampleType = item.Items[index].SampleType,
            //        sentDate = item.Items[index].SentDate,
            //        sizeDescription = item.SizeDescription,
            //        style = item.Style,
            //        sizeName = item.SizeName,
            //        garmentSectionName = (from aa in garmentSectionResult.data where aa.Id == item.Items[index].SectionId select aa.Name).FirstOrDefault()
            //    };
            //    sampleDtosList.Add(receiptSampleDto);
            //}

            foreach (var item in QuerySampleRequest.OrderByDescending(s => s.ReceivedDate).OrderByDescending(s => s.RoNoSample))
            {
                GarmentMonitoringReceiptSampleDto receiptSampleDto = new GarmentMonitoringReceiptSampleDto()
                {
                    buyer = item.BuyerCode + " - " + item.BuyerName,
                    color = item.Color,
                    quantity = item.Quantity,
                    receivedDate = item.ReceivedDate,
                    roNoSample = item.RoNoSample,
                    sampleCategory = item.SampleCategory,
                    sampleRequestDate = item.SampleRequestDate,
                    sampleRequestNo = item.SampleRequestNo,
                    sampleType = item.SampleType,
                    sentDate = item.SentDate,
                    sizeDescription = item.SizeDescription,
                    style = item.Style,
                    sizeName = item.SizeName,
                    sampleTo=item.SampleTo,
                    garmentSectionName = (from aa in garmentSectionResult.data where aa.Id == item.SectionId select aa.Name).FirstOrDefault()
                };
                sampleDtosList.Add(receiptSampleDto);
            }
            sampleViewModel.garmentMonitorings = sampleDtosList;
            return sampleViewModel;
        }
                
    }
}
