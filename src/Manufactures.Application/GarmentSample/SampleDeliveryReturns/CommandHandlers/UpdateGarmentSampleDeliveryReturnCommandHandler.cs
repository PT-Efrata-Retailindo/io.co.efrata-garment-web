using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleDeliveryReturns.CommandHandlers
{
    public class UpdateGarmentSampleDeliveryReturnCommandHandler : ICommandHandler<UpdateGarmentSampleDeliveryReturnCommand, GarmentSampleDeliveryReturn>
    {
        private readonly IGarmentSampleDeliveryReturnRepository _garmentSampleDeliveryReturnRepository;
        private readonly IGarmentSampleDeliveryReturnItemRepository _garmentSampleDeliveryReturnItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentSampleDeliveryReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleDeliveryReturnRepository = storage.GetRepository<IGarmentSampleDeliveryReturnRepository>();
            _garmentSampleDeliveryReturnItemRepository = storage.GetRepository<IGarmentSampleDeliveryReturnItemRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleDeliveryReturn> Handle(UpdateGarmentSampleDeliveryReturnCommand request, CancellationToken cancellaitonToken)
        {
            var requestTempItems = request.Items.Where(item => item.IsSave != true);

            request.Items = request.Items.Where(item => item.IsSave == true).ToList();
            var garmentSampleDeliveryReturn = _garmentSampleDeliveryReturnRepository.Find(o => o.Identity == request.Identity).FirstOrDefault();

            if (garmentSampleDeliveryReturn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Identity));

            garmentSampleDeliveryReturn.setDRNo(request.DRNo);
            garmentSampleDeliveryReturn.setRONo(request.RONo);
            garmentSampleDeliveryReturn.setArticle(request.Article);
            garmentSampleDeliveryReturn.setUnitDOId(request.UnitDOId);
            garmentSampleDeliveryReturn.setUnitDONo(request.UnitDONo);
            garmentSampleDeliveryReturn.setUENId(request.UENId);
            garmentSampleDeliveryReturn.setPreparingId(request.PreparingId);
            garmentSampleDeliveryReturn.setReturnDate(request.ReturnDate);
            garmentSampleDeliveryReturn.setReturnType(request.ReturnType);
            garmentSampleDeliveryReturn.setUnitId(new UnitDepartmentId(request.Unit.Id));
            garmentSampleDeliveryReturn.setUnitCode(request.Unit.Code);
            garmentSampleDeliveryReturn.setUnitName(request.Unit.Name);
            garmentSampleDeliveryReturn.setStorageId(new StorageId(request.Storage.Id));
            garmentSampleDeliveryReturn.setStorageCode(request.Storage.Code);
            garmentSampleDeliveryReturn.setStorageName(request.Storage.Name);
            garmentSampleDeliveryReturn.setIsUsed(request.IsUsed);

            var dbGarmentSampleDeliveryReturnItem = _garmentSampleDeliveryReturnItemRepository.Find(y => y.DRId == garmentSampleDeliveryReturn.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentSampleDeliveryReturnItem.Any(y => y.DRId == garmentSampleDeliveryReturn.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentSampleDeliveryReturnItem.Any(y => y.DRId == garmentSampleDeliveryReturn.Identity));


            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentSampleDeliveryReturnItem.Find(x => x.Identity == item.Id);
                dbItem.setUnitDOItemId(item.UnitDOItemId);
                dbItem.setUENItemId(item.UENItemId);
                dbItem.setPreparingItemId(item.PreparingItemId);
                dbItem.setProductId(new ProductId(item.Product.Id));
                dbItem.setProductCode(item.Product.Code);
                dbItem.setProductName(item.Product.Name);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setRONo(item.RONo);

                dbItem.setUomId(new UomId(item.Uom.Id));
                dbItem.setUomUnit(item.Uom.Unit);

                if (item.Product.Name == "FABRIC")
                {
                    var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Query.Where(x => x.Identity == Guid.Parse(item.PreparingItemId)).Select(s => new GarmentSamplePreparingItem(s)).Single();
                    garmentSamplePreparingItem.setRemainingQuantity(garmentSamplePreparingItem.RemainingQuantity + dbItem.Quantity - item.Quantity);
                    garmentSamplePreparingItem.SetModified();
                    await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
                }
                dbItem.setQuantity(item.Quantity);

                dbItem.SetModified();
                await _garmentSampleDeliveryReturnItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentSampleDeliveryReturnItem(Guid.NewGuid(), garmentSampleDeliveryReturn.Identity, x.UnitDOItemId, x.UENItemId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.RONo, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.Colour, x.Rack, x.Level, x.Box, x.Area)).ToList()
                .ForEach(async x => await _garmentSampleDeliveryReturnItemRepository.Update(x));

            foreach (var itemDeleted in requestTempItems)
            {
                var dbItemDeleted = dbGarmentSampleDeliveryReturnItem.Find(x => x.Identity == itemDeleted.Id);
                var deletedItems = dbGarmentSampleDeliveryReturnItem.Find(x => x.Identity == itemDeleted.Id);
                if (itemDeleted.Product.Name == "FABRIC")
                {
                    var garmentSamplePreparingItemDeleted = _garmentSamplePreparingItemRepository.Query.Where(x => x.Identity == Guid.Parse(itemDeleted.PreparingItemId)).Select(s => new GarmentSamplePreparingItem(s)).Single();
                    garmentSamplePreparingItemDeleted.setRemainingQuantity(garmentSamplePreparingItemDeleted.RemainingQuantity + dbItemDeleted.Quantity);
                    garmentSamplePreparingItemDeleted.SetModified();
                    await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItemDeleted);
                }
                deletedItems.Remove();
                await _garmentSampleDeliveryReturnItemRepository.Update(deletedItems);
            }

            garmentSampleDeliveryReturn.SetModified();

            await _garmentSampleDeliveryReturnRepository.Update(garmentSampleDeliveryReturn);

            _storage.Save();

            return garmentSampleDeliveryReturn;
        }
    }
}
