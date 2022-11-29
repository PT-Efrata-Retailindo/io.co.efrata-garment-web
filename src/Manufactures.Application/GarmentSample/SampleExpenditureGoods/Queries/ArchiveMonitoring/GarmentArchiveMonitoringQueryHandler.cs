using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries.ArchiveMonitoring
{
    public class GarmentArchiveMonitoringQueryHandler : IQueryHandler<GarmentArchiveMonitoringQuery, GarmentArchiveMonitoringViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleStockRepository _GarmentSampleStockRepository;
        private readonly IGarmentSampleRequestRepository GarmentSampleRequestRepository;
        public GarmentArchiveMonitoringQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _GarmentSampleStockRepository = storage.GetRepository<IGarmentSampleStockRepository>();
            GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }
        
        public async Task<GarmentArchiveMonitoringViewModel> Handle(GarmentArchiveMonitoringQuery request, CancellationToken cancellationToken)
        {
            List<GarmentArchiveMonitoringDto> monitoringDtos = new List<GarmentArchiveMonitoringDto>();
            monitoringDtos = (from a in _GarmentSampleStockRepository.Query
                           where a.ArchiveType == (request.type == null ? a.ArchiveType : request.type) 
                           && a.RONo == (request.roNo == null ? a.RONo : request.roNo)
                           && a.ComodityCode == (request.comodity == null ? a.ComodityCode : request.comodity)
                           select new GarmentArchiveMonitoringDto{
                               comodity= a.ComodityName,
                               archiveType= a.ArchiveType,
                               article= a.Article,
                               roNo= a.RONo,
                               size= a.SizeName,
                               qty= a.Quantity,
                               uom=a.UomUnit,
                               description= a.Description,
                               buyer= (from sample in GarmentSampleRequestRepository.Query where sample.RONoSample == a.RONo select sample.BuyerName).FirstOrDefault()
                           }).OrderByDescending(a=>a.roNo).ThenBy(b => b.archiveType).ToList();

            GarmentArchiveMonitoringViewModel listViewModel = new GarmentArchiveMonitoringViewModel();
            
            listViewModel.garmentMonitorings = monitoringDtos;
            return listViewModel;
        }
    }
}