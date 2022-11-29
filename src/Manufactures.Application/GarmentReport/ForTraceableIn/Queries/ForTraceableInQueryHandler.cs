using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace Manufactures.Application.GarmentReport.ForTraceableIn.Queries
{
    public class ForTraceableInQueryHandler : IQueryHandler<ForTraceableInQuery, ForTraceableInListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
        private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
        private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
        private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
        private readonly IGarmentSewingInRepository garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
        private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
        private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
        private readonly IGarmentFinishedGoodStockRepository garmentFinishedGoodStockRepository;

        public ForTraceableInQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
        }

        public async Task<ForTraceableInListViewModel> Handle(ForTraceableInQuery request, CancellationToken cancellationToken)
        {

            var Uen = request.uenitemid.Contains(",") ? request.uenitemid.Split(",").ToList() : new List<string> { request.uenitemid };

            List<int> UENItemId = Uen.Select(s => int.Parse(s)).ToList();

            var preparings = (from a in (from aa in garmentPreparingItemRepository.Query
                                         where UENItemId.Contains(aa.UENItemId)
                                         select aa)
                              join b in garmentPreparingRepository.Query on a.GarmentPreparingId equals b.Identity
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
                              join b in garmentCuttingOutRepository.Query on a.CutOutId equals b.Identity
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

            var sewingDO = (from a in (from aa in garmentSewingDOItemRepository.Query
                                      where cuttingOutId.Contains(aa.CuttingOutItemId)
                                      select aa)
                           join b in garmentLoadingItemRepository.Query on a.Identity equals b.SewingDOItemId
                           join c in garmentSewingInItemRepository.Query on b.Identity equals c.LoadingItemId
                           join d in garmentSewingOutItemRepository.Query on c.Identity equals d.SewingInItemId
                           join e in garmentFinishingInItemRepository.Query on d.Identity equals e.SewingOutItemId
                           join f in garmentFinishingInRepository.Query on e.FinishingInId equals f.Identity
                           select new { e.Identity, f.FinishingInType}).Distinct();

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
                          }).GroupBy(x => new { x.RoJob, x.FinishingInType, x.FinishingTo },(key,group) => new ForTraceableInDto
                          {
                              RoJob = key.RoJob,
                              FinishingInType = key.FinishingInType,
                              FinishingTo = key.FinishingTo,
                              FinishingOutQuantity = group.Sum(x=> x.FinishingOutQuantity)
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
                                }).GroupBy(x => new { x.ExpenditureType,x.RoJob,x.Invoice }, (key, group) => new ForTraceableInDto
                                {
                                    Invoice = key.Invoice,
                                    RoJob = key.RoJob,
                                    ExpenditureType = key.ExpenditureType,
                                    ExpenditureQuantity = group.Sum(x => x.ExpenditureQuantity)
                                });

            var Query = CutOutValue.Union(FinOutValue).Union(ExpendGoodValue).AsEnumerable();

            var querySUM = Query.GroupBy(x => new { x.RoJob, x.CutOutType, x.FinishingInType, x.FinishingTo, x.ExpenditureType,x.Invoice }, (key, group) => new ForTraceableInDto
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
                    //RoJob = a.RoJob == null ? "" : a.RoJob,
                    //CutOutType = a.CutOutType == null ? "":a.CutOutType,
                    //CutOutQuantity = a.CutOutQuantity == null ? 0 : a.CutOutQuantity,
                    //FinishingInType = a.FinishingInType == null ? "" : a.FinishingInType,
                    //FinishingOutQuantity = a.FinishingOutQuantity == null ? 0 :a.FinishingOutQuantity,
                    //ExpenditureType = a.ExpenditureType == null ? "" : a.ExpenditureType,
                    //ExpenditureQuantity = a.ExpenditureQuantity == null ? 0 : a.ExpenditureQuantity

                    RoJob = a.RoJob ,
                    CutOutType = a.CutOutType ,
                    CutOutQuantity = a.CutOutQuantity ,
                    FinishingInType = a.FinishingInType ,
                    FinishingTo = a.FinishingTo,
                    FinishingOutQuantity = a.FinishingOutQuantity ,
                    ExpenditureType = a.ExpenditureType ,
                    Invoice = a.Invoice,
                    ExpenditureQuantity = a.ExpenditureQuantity 
                };

                forTraceableInDtos.Add(forTraceableInDto);
            }

            viewModel.data = forTraceableInDtos;
           //ForTraceableInListViewModel viewModel = new ForTraceableInListViewModel();
            return viewModel;
        }
    }
}
