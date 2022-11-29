using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Dtos;
using System;

namespace Manufactures.ViewModels.GarmentAvalProducts
{
    public class CreateViewModelMapper
    {
        public GarmentAvalProduct Map(CreateViewModel viewModel)
        {
            return new GarmentAvalProduct(Guid.NewGuid(), viewModel.RONo, viewModel.Article, viewModel.AvalDate, new Domain.Shared.ValueObjects.UnitDepartmentId(viewModel.UnitId),viewModel.UnitCode,viewModel.UnitName);
        }

        public GarmentAvalProductItem MapItem(GarmentAvalProductItemDto viewModel, Guid headerId)
        {
            return new GarmentAvalProductItem(Guid.NewGuid(), headerId, new GarmentPreparingId(viewModel.PreparingId.Id), new GarmentPreparingItemId(viewModel.PreparingItemId.Id), new ProductId(viewModel.Product.Id), viewModel.Product.Code, viewModel.Product.Name, viewModel.DesignColor, viewModel.Quantity, new UomId(viewModel.Uom.Id), viewModel.Uom.Unit, viewModel.BasicPrice,viewModel.IsReceived);
        }
    }
}