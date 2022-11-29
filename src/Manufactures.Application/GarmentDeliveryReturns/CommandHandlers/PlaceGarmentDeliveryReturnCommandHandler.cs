using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentDeliveryReturns.CommandHandlers
{
    public class PlaceGarmentDeliveryReturnCommandHandler : ICommandHandler<PlaceGarmentDeliveryReturnCommand, GarmentDeliveryReturn>
    {
        private readonly IGarmentDeliveryReturnRepository _garmentDeliveryReturnRepository;
        private readonly IGarmentDeliveryReturnItemRepository _garmentDeliveryReturnItemRepository;
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentDeliveryReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
            _garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        public async Task<GarmentDeliveryReturn> Handle(PlaceGarmentDeliveryReturnCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();
            var garmentDeliveryReturn = _garmentDeliveryReturnRepository.Find(o =>
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
            List<GarmentDeliveryReturnItem> garmentDeliveryReturnItem = new List<GarmentDeliveryReturnItem>();
            if (garmentDeliveryReturn == null)
            {
                garmentDeliveryReturn = new GarmentDeliveryReturn(Guid.NewGuid(), GenerateDRNo(request), request.RONo, request.Article, request.UnitDOId, request.UnitDONo, request.UENId, request.PreparingId, request.ReturnDate, request.ReturnType, new UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name, new StorageId(request.Storage.Id), request.Storage.Name, request.Storage.Code, request.IsUsed);
                request.Items.Select(x => new GarmentDeliveryReturnItem(Guid.NewGuid(), garmentDeliveryReturn.Identity, x.UnitDOItemId, x.UENItemId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.RONo, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit)).ToList()
                    .ForEach(async x => await _garmentDeliveryReturnItemRepository.Update(x));
            }
            
            foreach (var itemDeliveryReturn in request.Items)
            {
                if(itemDeliveryReturn.Product.Name == "FABRIC")
                {
                    var garmentPreparingItem = _garmentPreparingItemRepository.Find(o => o.Identity == Guid.Parse(itemDeliveryReturn.PreparingItemId)).Single();

                    garmentPreparingItem.setRemainingQuantityZeroValue(garmentPreparingItem.RemainingQuantity - itemDeliveryReturn.Quantity);

                    garmentPreparingItem.SetModified();
                    await _garmentPreparingItemRepository.Update(garmentPreparingItem);
                }
            }

            garmentDeliveryReturn.SetModified();

            await _garmentDeliveryReturnRepository.Update(garmentDeliveryReturn);

            _storage.Save();

            return garmentDeliveryReturn;

        }

        private string GenerateDRNo(PlaceGarmentDeliveryReturnCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");

            var prefix = $"DR{request.Unit.Code}{year}{month}{day}";

            var lastDRNo = _garmentDeliveryReturnRepository.Query.Where(w => w.DRNo.StartsWith(prefix)).OrderByDescending(o => o.DRNo).Select(s => int.Parse(s.DRNo.Replace(prefix, ""))).FirstOrDefault();
            var drNo = $"{prefix}{(lastDRNo + 1).ToString("d4")}";

            return drNo;
        }
    }
}