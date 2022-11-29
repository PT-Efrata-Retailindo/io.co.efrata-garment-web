using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Dtos;
using System;

namespace Manufactures.ViewModels.GarmentDeliveryReturns
{
    public class CreateViewModelMapper
    {
        public GarmentDeliveryReturn Map(CreateViewModel viewModel)
        {
            return new GarmentDeliveryReturn(Guid.NewGuid(), viewModel.DRNo, viewModel.RONo, viewModel.Article, viewModel.UnitDOId, viewModel.UnitDONo, viewModel.UENId, viewModel.PreparingId, viewModel.ReturnDate,
               viewModel.ReturnType, new UnitDepartmentId(viewModel.UnitId), viewModel.UnitCode, viewModel.UnitName, new StorageId(viewModel.StorageId), viewModel.StorageName, viewModel.StorageCode, viewModel.IsUsed);
        }

        public GarmentDeliveryReturnItem MapItem(GarmentDeliveryReturnItemDto viewModel, Guid headerId)
        {
            return new GarmentDeliveryReturnItem(Guid.NewGuid(), headerId, viewModel.UnitDOItemId, viewModel.UENItemId, viewModel.PreparingItemId, new ProductId(viewModel.Product.Id), viewModel.Product.Code, viewModel.Product.Name, viewModel.DesignColor, viewModel.RONo, viewModel.Quantity, new UomId(viewModel.Uom.Id), viewModel.Uom.Unit);
        }
    }
}