using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers
{
    public class UpdateGarmentSamplePreparingCommandHandler : ICommandHandler<UpdateGarmentSamplePreparingCommand, GarmentSamplePreparing>
    {
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentSamplePreparingCommandHandler(IStorage storage)
        {
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSamplePreparing> Handle(UpdateGarmentSamplePreparingCommand request, CancellationToken cancellaitonToken)
        {
            var garmentSamplePreparing = _garmentSamplePreparingRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentSamplePreparing == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            garmentSamplePreparing.setUENId(request.UENId);
            garmentSamplePreparing.setUENNo(request.UENNo);
            garmentSamplePreparing.SetUnitId(new UnitDepartmentId(request.Unit.Id));
            garmentSamplePreparing.setUnitCode(request.Unit.Code);
            garmentSamplePreparing.setUnitName(request.Unit.Name);
            garmentSamplePreparing.setProcessDate(request.ProcessDate);
            garmentSamplePreparing.setRONo(request.RONo);
            garmentSamplePreparing.setArticle(request.Article);
            garmentSamplePreparing.setIsCuttingIN(request.IsCuttingIn);

            var dbGarmentSamplePreparing = _garmentSamplePreparingItemRepository.Find(y => y.GarmentSamplePreparingId == garmentSamplePreparing.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentSamplePreparing.Any(y => y.GarmentSamplePreparingId == garmentSamplePreparing.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentSamplePreparing.Any(y => y.GarmentSamplePreparingId == garmentSamplePreparing.Identity));
            var deletedItems = dbGarmentSamplePreparing.Where(x => !request.Items.Any(y => y.GarmentSamplePreparingId == garmentSamplePreparing.Identity));

            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentSamplePreparing.Find(x => x.Identity == item.Identity);
                dbItem.setBasicPrice(item.BasicPrice);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setFabricType(item.FabricType);
                dbItem.setProduct(new ProductId(item.Product.Id));
                dbItem.setProductCode(item.Product.Code);
                dbItem.setProductName(item.Product.Name);
                dbItem.setQuantity(item.Quantity);
                dbItem.setRemainingQuantity(item.RemainingQuantity);
                dbItem.setUenItemId(item.UENItemId);
                dbItem.setUomId(new UomId(item.Uom.Id));
                dbItem.setUomUnit(item.Uom.Unit);
                await _garmentSamplePreparingItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentSamplePreparingItem(Guid.NewGuid(), x.UENItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.FabricType, x.RemainingQuantity, x.BasicPrice, garmentSamplePreparing.Identity, x.ROSource)).ToList()
                .ForEach(async x => await _garmentSamplePreparingItemRepository.Update(x));

            foreach (var item in deletedItems)
            {
                item.SetDeleted();
                await _garmentSamplePreparingItemRepository.Update(item);
            }

            garmentSamplePreparing.SetModified();

            await _garmentSamplePreparingRepository.Update(garmentSamplePreparing);

            _storage.Save();

            return garmentSamplePreparing;
        }
    }
}
