using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingAdjustments;
using Manufactures.Domain.GarmentCuttingAdjustments.Commands;
using Manufactures.Domain.GarmentCuttingAdjustments.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Application.GarmentCuttingAdjustments.CommandHandlers
{
    public class PlaceGarmentCuttingAdjustmentCommandHandler : ICommandHandler<PlaceGarmentCuttingAdjustmentCommand, GarmentCuttingAdjustment>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingAdjustmentRepository _garmentCuttingAdjustmentRepository;
        private readonly IGarmentCuttingAdjustmentItemRepository _garmentCuttingAdjustmentItemRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;

        public PlaceGarmentCuttingAdjustmentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingAdjustmentRepository = storage.GetRepository<IGarmentCuttingAdjustmentRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingAdjustmentItemRepository = storage.GetRepository<IGarmentCuttingAdjustmentItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        public async Task<GarmentCuttingAdjustment> Handle(PlaceGarmentCuttingAdjustmentCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item=>item.IsSave).ToList();

            GarmentCuttingAdjustment garmentCuttingAdj = new GarmentCuttingAdjustment(
                Guid.NewGuid(),
                GenerateCutAdjustmentNo(request),
                request.CutInNo,
                request.CutInId,
                request.RONo,
                request.TotalFC,
                request.TotalActualFC,
                request.TotalQuantity,
                request.TotalActualQuantity,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.AdjustmentDate.GetValueOrDefault(),
                request.AdjustmentDesc
            );

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentCuttingAdjustmentItem garmentCuttingAdjustmentItem = new GarmentCuttingAdjustmentItem(
                        Guid.NewGuid(),
                        garmentCuttingAdj.Identity,
                        item.CutInDetailId,
                        item.PreparingItemId,
                        item.FC,
                        item.ActualFC,
                        item.Quantity,
                        item.ActualQuantity
                    );
                    await _garmentCuttingAdjustmentItemRepository.Update(garmentCuttingAdjustmentItem);

                    var cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == item.CutInDetailId).Select(s => new GarmentCuttingInDetail(s)).Single();
                    cuttingInDetail.SetPreparingQuantity((double)item.ActualQuantity);
                    cuttingInDetail.SetFC((double)item.ActualFC);
                    cuttingInDetail.Modify();
                    await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                    
                    var garmentPreparingItem = _garmentPreparingItemRepository.Query.IgnoreQueryFilters().Where(i => (i.Deleted == true && i.DeletedBy == "LUCIA") || (i.Deleted == false)).Where(x => x.Identity == item.PreparingItemId).Select(s => new GarmentPreparingItem(s)).Single();
                    garmentPreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentPreparingItem.RemainingQuantity + (item.Quantity - item.ActualQuantity)));
                    garmentPreparingItem.SetModified();

                    await _garmentPreparingItemRepository.Update(garmentPreparingItem);
                }
            }
            var cuttingIn = _garmentCuttingInRepository.Query.Where(x => x.Identity == request.CutInId).Select(s => new GarmentCuttingIn(s)).Single();
            cuttingIn.SetFC((double)request.TotalActualFC);
            cuttingIn.Modify();
            await _garmentCuttingInRepository.Update(cuttingIn);

            await _garmentCuttingAdjustmentRepository.Update(garmentCuttingAdj);

            _storage.Save();

            return garmentCuttingAdj;
        }

        private string GenerateCutAdjustmentNo(PlaceGarmentCuttingAdjustmentCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"ADJC{request.Unit.Code.Trim()}{year}{month}";

            var lastCutAdjNo = _garmentCuttingAdjustmentRepository.Query.Where(w => w.AdjustmentNo.StartsWith(prefix))
                .OrderByDescending(o => o.AdjustmentNo)
                .Select(s => int.Parse(s.AdjustmentNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutAdjNo = $"{prefix}{(lastCutAdjNo + 1).ToString("D4")}";

            return CutAdjNo;
        }
    }
}
