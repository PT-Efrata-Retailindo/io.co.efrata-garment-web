using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleDeliveryReturns.CommandHandlers
{
    public class PlaceGarmentSampleDeliveryReturnCommandHandler : ICommandHandler<PlaceGarmentSampleDeliveryReturnCommand, GarmentSampleDeliveryReturn>
    {
        private readonly IGarmentSampleDeliveryReturnRepository _garmentSampleDeliveryReturnRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository _garmentSampleDeliveryReturnItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentSampleDeliveryReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleDeliveryReturnRepository = storage.GetRepository<IGarmentSampleDeliveryReturnRepository>();
            _garmentSampleDeliveryReturnItemRepository = storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleDeliveryReturn> Handle(PlaceGarmentSampleDeliveryReturnCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();
            var garmentSampleDeliveryReturn = _garmentSampleDeliveryReturnRepository.Find(o =>
                                   o.DRNo == request.DRNo &&
                                   o.RONo == request.RONo &&
                                   o.Article == request.Article &&
                                   o.UnitDOId == request.UnitDOId &&
                                   o.UnitDONo == request.UnitDONo &&
                                   o.UENId == request.UENId &&
                                   o.PreparingId == request.PreparingId &&
                                   o.ReturnDate == request.ReturnDate &&
                                   o.ReturnType == request.ReturnType &&
                                   o.UnitId == request.Unit.Id &&
                                   o.UnitCode == request.Unit.Code &&
                                   o.UnitName == request.Unit.Name &&
                                   o.StorageId == request.Storage.Id &&
                                   o.StorageCode == request.Storage.Code &&
                                   o.StorageName == request.Storage.Name &&
                                   o.IsUsed == request.IsUsed).FirstOrDefault();
            List<GarmentSampleDeliveryReturnItem> garmentSampleDeliveryReturnItem = new List<GarmentSampleDeliveryReturnItem>();
            if (garmentSampleDeliveryReturn == null)
            {
                garmentSampleDeliveryReturn = new GarmentSampleDeliveryReturn(Guid.NewGuid(), GenerateDRNo(request), request.RONo, request.Article, request.UnitDOId, request.UnitDONo, request.UENId, request.PreparingId, request.ReturnDate, request.ReturnType, new UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name, new StorageId(request.Storage.Id), request.Storage.Name, request.Storage.Code, request.IsUsed);
                request.Items.Select(x => new GarmentSampleDeliveryReturnItem(Guid.NewGuid(), garmentSampleDeliveryReturn.Identity, x.UnitDOItemId, x.UENItemId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.RONo, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit)).ToList()
                    .ForEach(async x => await _garmentSampleDeliveryReturnItemRepository.Update(x));
            }

            foreach (var itemDeliveryReturn in request.Items)
            {
                if (itemDeliveryReturn.Product.Name == "FABRIC")
                {
                    var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Find(o => o.Identity == Guid.Parse(itemDeliveryReturn.PreparingItemId)).Single();

                    garmentSamplePreparingItem.setRemainingQuantityZeroValue(garmentSamplePreparingItem.RemainingQuantity - itemDeliveryReturn.Quantity);

                    garmentSamplePreparingItem.SetModified();
                    await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
                }
            }

            garmentSampleDeliveryReturn.SetModified();

            await _garmentSampleDeliveryReturnRepository.Update(garmentSampleDeliveryReturn);

            _storage.Save();

            return garmentSampleDeliveryReturn;

        }

        private string GenerateDRNo(PlaceGarmentSampleDeliveryReturnCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");

            var prefix = $"DR{request.Unit.Code}{year}{month}{day}";

            var lastDRNo = _garmentSampleDeliveryReturnRepository.Query.Where(w => w.DRNo.StartsWith(prefix)).OrderByDescending(o => o.DRNo).Select(s => int.Parse(s.DRNo.Replace(prefix, ""))).FirstOrDefault();
            var drNo = $"{prefix}{(lastDRNo + 1).ToString("d4")}";

            return drNo;
        }
    }
}
