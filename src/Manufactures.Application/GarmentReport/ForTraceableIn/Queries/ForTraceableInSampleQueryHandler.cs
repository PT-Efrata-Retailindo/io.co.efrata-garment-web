using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace Manufactures.Application.GarmentReport.ForTraceableIn.Queries
{
    public class ForTraceableInSampleQueryHandler : IQueryHandler<ForTraceableInSampleQuery, ForTraceableInListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository garmentCuttingOutItemRepository;
        private readonly IGarmentSampleSewingInRepository garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository garmentSewingInItemRepository;
        private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentSampleFinishingInRepository garmentFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository garmentFinishingInItemRepository;
        private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentSampleExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentSampleFinishedGoodStockHistoryRepository garmentFinishedGoodStockRepository;

        public ForTraceableInSampleQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
            garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockHistoryRepository>();
        }

        public async Task<ForTraceableInListViewModel> Handle(ForTraceableInSampleQuery request, CancellationToken cancellationToken)
        {

            var Uen = request.uenitemid.Contains(",") ? request.uenitemid.Split(",").ToList() : new List<string> { request.uenitemid };

            List<int> UENItemId = Uen.Select(s => int.Parse(s)).ToList();

            var preparings = (from a in (from aa in garmentPreparingItemRepository.Query
                                         where UENItemId.Contains(aa.UENItemId)
                                         select aa)
                              join b in garmentPreparingRepository.Query on a.GarmentSamplePreparingId equals b.Identity
                              select new { b.Identity }).Distinct();

            var preparingId = preparings.Select(s => Guid.Parse(s.Identity.ToString())).ToList();

            var cuttingIns = (from a in garmentCuttingInRepository.Query
                              join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                              where preparingId.Contains(b.PreparingId)
                              select new { a.Identity }).Distinct();

            var cuttingInId = cuttingIns.Select(s => Guid.Parse(s.Identity.ToString())).ToList();

            var cuttingOuts = (from a in (from aa in garmentCuttingOutItemRepository.Query
                                          where cuttingInId.Contains(aa.CuttingInId)
                                          select aa)
                               join b in garmentCuttingOutRepository.Query on a.CuttingOutId equals b.Identity
                               select new
                               {
                                   Identity = a.Identity,
                                   RoJob = b.RONo,
                                   CutOutType = b.CuttingOutType,
                                   CutOutQuantity = a.TotalCuttingOut
                               });

            var cuttingOutId = cuttingOuts.Select(x => Guid.Parse(x.Identity.ToString())).Distinct().ToList();

            var CutOutValue = cuttingOuts.GroupBy(x => new { x.RoJob, x.CutOutType }, (key, group) => new ForTraceableInDto
            {
                RoJob = key.RoJob,
                CutOutType = key.CutOutType,
                CutOutQuantity = group.Sum(x => x.CutOutQuantity)

            });

            var sewingDO = (from a in (from aa in garmentSewingInItemRepository.Query
                                       where cuttingOutId.Contains(aa.CuttingOutItemId)
                                       select aa)
                            join d in garmentSewingOutItemRepository.Query on a.Identity equals d.SampleSewingInId
                            join e in garmentFinishingInItemRepository.Query on d.Identity equals e.SewingOutItemId
                            join f in garmentFinishingInRepository.Query on e.FinishingInId equals f.Identity
                            select new { e.Identity, f.FinishingInType }).Distinct();

            var FinInId = sewingDO.Select(x => Guid.Parse(x.Identity.ToString())).ToList();

            var FinOutValue = (from a in (from aa in garmentFinishingOutItemRepository.Query
                                          where FinInId.Contains(aa.FinishingInItemId)
                                          select aa)
                               join b in garmentFinishingOutRepository.Query on a.FinishingOutId equals b.Identity
                               join c in sewingDO on a.FinishingInItemId equals c.Identity
                               select new ForTraceableInDto
                               {
                                   RoJob = b.RONo,
                                   FinishingInType = c.FinishingInType,
                                   FinishingTo = b.FinishingTo,
                                   FinishingOutQuantity = a.Quantity
                               }).GroupBy(x => new { x.RoJob, x.FinishingInType, x.FinishingTo }, (key, group) => new ForTraceableInDto
                               {
                                   RoJob = key.RoJob,
                                   FinishingInType = key.FinishingInType,
                                   FinishingTo = key.FinishingTo,
                                   FinishingOutQuantity = group.Sum(x => x.FinishingOutQuantity)
                               });

            var ro = FinOutValue.Select(x => x.RoJob).Distinct().ToList();

            var ExpendGoodValue = (from a in (from aa in garmentFinishedGoodStockRepository.Query
                                              where ro.Contains(aa.RONo)
                                              select aa)
                                   join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.FinishedGoodStockId
                                   join c in garmentExpenditureGoodRepository.Query on b.ExpenditureGoodId equals c.Identity
                                   select new ForTraceableInDto
                                   {
                                       RoJob = a.RONo,
                                       Invoice = c.Invoice,
                                       ExpenditureType = c.ExpenditureType,
                                       ExpenditureQuantity = b.Quantity
                                   }).GroupBy(x => new { x.ExpenditureType, x.RoJob, x.Invoice }, (key, group) => new ForTraceableInDto
                                   {
                                       Invoice = key.Invoice,
                                       RoJob = key.RoJob,
                                       ExpenditureType = key.ExpenditureType,
                                       ExpenditureQuantity = group.Sum(x => x.ExpenditureQuantity)
                                   });

            var Query = CutOutValue.Union(FinOutValue).Union(ExpendGoodValue).AsEnumerable();

            var querySUM = Query.GroupBy(x => new { x.RoJob, x.CutOutType, x.FinishingInType, x.FinishingTo, x.ExpenditureType, x.Invoice }, (key, group) => new ForTraceableInDto
            {
                RoJob = key.RoJob,
                CutOutType = key.CutOutType,
                CutOutQuantity = group.Sum(x => x.CutOutQuantity),
                FinishingInType = key.FinishingInType,
                FinishingTo = key.FinishingTo,
                FinishingOutQuantity = group.Sum(x => x.FinishingOutQuantity),
                ExpenditureType = key.ExpenditureType,
                Invoice = key.Invoice,
                ExpenditureQuantity = group.Sum(x => x.ExpenditureQuantity)
            });

            ForTraceableInListViewModel viewModel = new ForTraceableInListViewModel();
            List<ForTraceableInDto> forTraceableInDtos = new List<ForTraceableInDto>();
            foreach (var a in querySUM)
            {
                ForTraceableInDto forTraceableInDto = new ForTraceableInDto()
                {
                    RoJob = a.RoJob,
                    CutOutType = a.CutOutType,
                    CutOutQuantity = a.CutOutQuantity,
                    FinishingInType = a.FinishingInType,
                    FinishingTo = a.FinishingTo,
                    FinishingOutQuantity = a.FinishingOutQuantity,
                    ExpenditureType = a.ExpenditureType,
                    Invoice = a.Invoice,
                    ExpenditureQuantity = a.ExpenditureQuantity
                };

                forTraceableInDtos.Add(forTraceableInDto);
            }

            viewModel.data = forTraceableInDtos;
            return viewModel;
        }
    }
}
