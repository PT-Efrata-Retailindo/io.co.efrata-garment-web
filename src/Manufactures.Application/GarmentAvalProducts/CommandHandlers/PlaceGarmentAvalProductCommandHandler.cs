using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class PlaceGarmentAvalProductCommandHandler : ICommandHandler<PlaceGarmentAvalProductCommand, GarmentAvalProduct>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentAvalProductCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        public async Task<GarmentAvalProduct> Handle(PlaceGarmentAvalProductCommand request, CancellationToken cancellationToken)
        {
            //var garmentAvalProduct = _garmentAvalProductRepository.Find(o =>
            //                       o.RONo == request.RONo &&
            //                       o.Article == request.Article &&
            //                       o.AvalDate == request.AvalDate).FirstOrDefault();
            //List<GarmentAvalProductItem> garmentAvalProductItem = new List<GarmentAvalProductItem>();
            //if (garmentAvalProduct == null)
            //{
            GarmentAvalProduct garmentAvalProduct = new GarmentAvalProduct(Guid.NewGuid(), request.RONo, request.Article, request.AvalDate, new Domain.Shared.ValueObjects.UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name);
                request.Items.Select(x => new GarmentAvalProductItem(Guid.NewGuid(), garmentAvalProduct.Identity, x.PreparingId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.BasicPrice,x.IsReceived)).ToList()
                    .ForEach(async x => await _garmentAvalProductItemRepository.Update(x));
            //}

            foreach(var itemPreparing in request.Items)
            {
                var garmentPreparingItem = _garmentPreparingItemRepository.Find(o => o.Identity == Guid.Parse(itemPreparing.PreparingItemId.Value)).Single();

                garmentPreparingItem.setRemainingQuantityZeroValue(garmentPreparingItem.RemainingQuantity - itemPreparing.Quantity);
                
                garmentPreparingItem.SetModified();
                await _garmentPreparingItemRepository.Update(garmentPreparingItem);

            }

            garmentAvalProduct.SetModified();

            await _garmentAvalProductRepository.Update(garmentAvalProduct);

            _storage.Save();

            return garmentAvalProduct;

        }
    }
}