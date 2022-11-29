using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubconCuttingOuts.CommandHandlers
{
    public class RemoveGarmentSubconCuttingOutCommandHandler : ICommandHandler<RemoveGarmentSubconCuttingOutCommand, GarmentSubconCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentSubconCuttingRelationRepository _garmentSubconCuttingRelationRepository;

        public RemoveGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentSubconCuttingRepository>();
            _garmentSubconCuttingRelationRepository = storage.GetRepository<IGarmentSubconCuttingRelationRepository>();
        }

        public async Task<GarmentSubconCuttingOut> Handle(RemoveGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            var cutOut = _garmentCuttingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCuttingOut(o)).Single();
            
            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<string, double> cuttingSubconToBeUpdated = new Dictionary<string, double>();
            Dictionary<string, List<Guid>> cuttingSubconToBeUpdatedId = new Dictionary<string, List<Guid>>();

            _garmentCuttingOutItemRepository.Find(o => o.CutOutId == cutOut.Identity).ForEach(async cutOutItem =>
            {
                _garmentCuttingOutDetailRepository.Find(o => o.CutOutItemId == cutOutItem.Identity).ForEach(async cutOutDetail =>
                {
                    string key = cutOut.RONo + "~" + cutOutDetail.SizeId.Value.ToString() + "~" + cutOutDetail.SizeName + "~"
                        + cutOutItem.ProductId.Value.ToString() + "~" + cutOutItem.ProductCode + "~" + cutOutItem.ProductName + "~"
                        + cutOut.ComodityId.Value.ToString() + "~" + cutOut.ComodityCode + "~" + cutOut.ComodityName + "~"
                        + cutOutItem.DesignColor + "~" + cutOutDetail.Remark + "~" + cutOutDetail.BasicPrice;

                    if (cuttingSubconToBeUpdated.ContainsKey(key))
                    {
                        cuttingSubconToBeUpdated[key] += cutOutDetail.CuttingOutQuantity;
                        cuttingSubconToBeUpdatedId[key].Add(cutOutDetail.Identity);
                    }
                    else
                    {
                        cuttingSubconToBeUpdated.Add(key, cutOutDetail.CuttingOutQuantity);
                        cuttingSubconToBeUpdatedId.Add(key, new List<Guid> { cutOutDetail.Identity });
                    }

                    if (cuttingInDetailToBeUpdated.ContainsKey(cutOutItem.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[cutOutItem.CuttingInDetailId] += cutOutDetail.RemainingQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(cutOutItem.CuttingInDetailId, cutOutDetail.RemainingQuantity);
                    }

                    cutOutDetail.Remove();
                    await _garmentCuttingOutDetailRepository.Update(cutOutDetail);
                });

                cutOutItem.Remove();
                await _garmentCuttingOutItemRepository.Update(cutOutItem);
            });

            foreach (var cuttingInItem in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInItem.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity + cuttingInItem.Value);
                garmentCuttingInDetail.Modify();
                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            foreach (var subconCutting in cuttingSubconToBeUpdated)
            {
                var RONo = subconCutting.Key.Split("~")[0];
                var SizeId = subconCutting.Key.Split("~")[1];
                var SizeName = subconCutting.Key.Split("~")[2];
                var ProductId = subconCutting.Key.Split("~")[3];
                var ProductCode = subconCutting.Key.Split("~")[4];
                var ProductName = subconCutting.Key.Split("~")[5];
                var ComodityId = subconCutting.Key.Split("~")[6];
                var ComodityCode = subconCutting.Key.Split("~")[7];
                var ComodityName = subconCutting.Key.Split("~")[8];
                var designColor = subconCutting.Key.Split("~")[9];
                var remark = subconCutting.Key.Split("~")[10];
                var basicPrice = subconCutting.Key.Split("~")[11];

                GarmentSubconCutting garmentSubconCutting = _garmentSubconCuttingRepository.Query.Where(a => a.RONo == RONo && a.SizeId == Convert.ToInt32(SizeId) && a.ComodityId == Convert.ToInt32(ComodityId) && a.ProductId == Convert.ToInt32(ProductId) && a.Remark == remark && a.DesignColor == designColor && a.BasicPrice == Convert.ToDouble(basicPrice)).Select(a => new GarmentSubconCutting(a)).FirstOrDefault();
                
                garmentSubconCutting.SetQuantity(garmentSubconCutting.Quantity - subconCutting.Value);
                garmentSubconCutting.Modify();
                await _garmentSubconCuttingRepository.Update(garmentSubconCutting);

                foreach (var detailId in cuttingSubconToBeUpdatedId[subconCutting.Key] ?? new List<Guid>())
                {
                    GarmentSubconCuttingRelation garmentSubconCuttingRelation = _garmentSubconCuttingRelationRepository.Query.Where(w => w.GarmentSubconCuttingId == garmentSubconCutting.Identity && w.GarmentCuttingOutDetailId == detailId).Select(s => new GarmentSubconCuttingRelation(s)).FirstOrDefault();
                    if (garmentSubconCuttingRelation != null)
                    {
                        garmentSubconCuttingRelation.Remove();
                        await _garmentSubconCuttingRelationRepository.Update(garmentSubconCuttingRelation);
                    }
                }
            }

            cutOut.Remove();
            await _garmentCuttingOutRepository.Update(cutOut);

            _storage.Save();

            return cutOut;
        }
    }
}
