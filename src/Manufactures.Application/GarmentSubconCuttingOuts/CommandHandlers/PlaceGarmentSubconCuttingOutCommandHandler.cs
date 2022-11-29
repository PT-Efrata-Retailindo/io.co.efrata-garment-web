using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubconCuttingOuts.CommandHandlers
{
    public class PlaceGarmentSubconCuttingOutCommandHandler : ICommandHandler<PlaceGarmentSubconCuttingOutCommand, GarmentSubconCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSubconCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSubconCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSubconCuttingRepository _garmentSubconCuttingRepository;
        //private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentSubconCuttingRelationRepository _garmentSubconCuttingRelationRepository;

        public PlaceGarmentSubconCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSubconCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSubconCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentSubconCuttingRepository>();
            //_garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentSubconCuttingRelationRepository = storage.GetRepository<IGarmentSubconCuttingRelationRepository>();
        }

        public async Task<GarmentSubconCuttingOut> Handle(PlaceGarmentSubconCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentSubconCuttingOut garmentCuttingOut = new GarmentSubconCuttingOut(
                Guid.NewGuid(),
                GenerateCutOutNo(request),
                request.CuttingOutType,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.CuttingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.EPOId,
                request.EPOItemId,
                request.POSerialNumber,
                request.IsUsed
            );

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<string, double> cuttingSubconToBeUpdated = new Dictionary<string, double>();
            Dictionary<string, List<Guid>> cuttingSubconToBeUpdatedId = new Dictionary<string, List<Guid>>();

            foreach (var item in request.Items)
            {
                GarmentSubconCuttingOutItem garmentCuttingOutItem = new GarmentSubconCuttingOutItem(
                    Guid.NewGuid(),
                    item.CuttingInId,
                    item.CuttingInDetailId,
                    garmentCuttingOut.Identity,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.TotalCuttingOutQuantity
                );

                foreach (var detail in item.Details)
                {
                    var detailId = Guid.NewGuid();
                    GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(
                        detailId,
                        garmentCuttingOutItem.Identity,
                        new SizeId(detail.Size.Id),
                        detail.Size.Size,
                        detail.Color,
                        detail.CuttingOutQuantity,
                        detail.CuttingOutQuantity,
                        new UomId(detail.CuttingOutUom.Id),
                        detail.CuttingOutUom.Unit,
                        detail.BasicPrice,
                        detail.Price,
                        detail.Remark.ToUpper()
                    );

                    string key = request.RONo + "~" + detail.Size.Id.ToString() + "~" + detail.Size.Size + "~" 
                        + item.Product.Id.ToString() + "~" + item.Product.Code + "~" + item.Product.Name + "~" 
                        + request.Comodity.Id.ToString() + "~" + request.Comodity.Code + "~" +request.Comodity.Name + "~" 
                        + item.DesignColor + "~" + detail.Remark.ToUpper() + "~" + detail.BasicPrice;

                    if (cuttingSubconToBeUpdated.ContainsKey(key))
                    {
                        cuttingSubconToBeUpdated[key] += detail.CuttingOutQuantity;
                        cuttingSubconToBeUpdatedId[key].Add(detailId);
                    }
                    else
                    {
                        cuttingSubconToBeUpdated.Add(key, detail.CuttingOutQuantity);
                        cuttingSubconToBeUpdatedId.Add(key, new List<Guid> { detailId });
                    }

                    if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                    }

                    await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                }

                await _garmentCuttingOutItemRepository.Update(garmentCuttingOutItem);
            }

            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                garmentCuttingInDetail.Modify();

                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            foreach(var subconCutting in cuttingSubconToBeUpdated)
            {
                var RONo = subconCutting.Key.Split("~")[0];
                var SizeId= subconCutting.Key.Split("~")[1];
                var SizeName = subconCutting.Key.Split("~")[2];
                var ProductId= subconCutting.Key.Split("~")[3];
                var ProductCode = subconCutting.Key.Split("~")[4];
                var ProductName= subconCutting.Key.Split("~")[5];
                var ComodityId =subconCutting.Key.Split("~")[6];
                var ComodityCode= subconCutting.Key.Split("~")[7];
                var ComodityName= subconCutting.Key.Split("~")[8];
                var designColor= subconCutting.Key.Split("~")[9];
                var remark= subconCutting.Key.Split("~")[10];
                var basicPrice = subconCutting.Key.Split("~")[11];

                GarmentSubconCutting garmentSubconCutting = _garmentSubconCuttingRepository.Query.Where(a => a.RONo == RONo && a.SizeId == Convert.ToInt32(SizeId) && a.ComodityId== Convert.ToInt32(ComodityId)&& a.ProductId== Convert.ToInt32(ProductId) && a.Remark==remark&& a.DesignColor==designColor&& a.BasicPrice== Convert.ToDouble(basicPrice)).Select(a => new GarmentSubconCutting(a)).FirstOrDefault();
                if (garmentSubconCutting == null)
                {
                    garmentSubconCutting = new GarmentSubconCutting(
                        Guid.NewGuid(),
                        request.RONo,
                        new SizeId( Convert.ToInt32(SizeId)),
                        SizeName,
                        subconCutting.Value,
                        new ProductId(Convert.ToInt32(ProductId)),
                        ProductCode,
                        ProductName,
                        new GarmentComodityId(Convert.ToInt32(ComodityId)),
                        ComodityCode,
                        ComodityName,
                        designColor,
                        remark,
                        Convert.ToDouble(basicPrice)
                    );
                    await _garmentSubconCuttingRepository.Update(garmentSubconCutting);
                }
                else
                {
                    garmentSubconCutting.SetQuantity(garmentSubconCutting.Quantity + subconCutting.Value);
                    garmentSubconCutting.Modify();
                    await _garmentSubconCuttingRepository.Update(garmentSubconCutting);
                }

                foreach (var detailId in cuttingSubconToBeUpdatedId[subconCutting.Key] ?? new List<Guid>())
                {
                    GarmentSubconCuttingRelation garmentSubconCuttingRelation = new GarmentSubconCuttingRelation(Guid.NewGuid(), garmentSubconCutting.Identity, detailId);
                    await _garmentSubconCuttingRelationRepository.Update(garmentSubconCuttingRelation);
                }
            }

            await _garmentCuttingOutRepository.Update(garmentCuttingOut);

            _storage.Save();

            return garmentCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentSubconCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var unitcode = request.UnitFrom.Code;

            var prefix = $"CS{unitcode}{year}{month}";

            var lastCutOutNo = _garmentCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }

        

    }
}
