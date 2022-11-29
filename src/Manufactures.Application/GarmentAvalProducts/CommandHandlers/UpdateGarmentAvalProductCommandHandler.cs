using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class UpdateGarmentAvalProductCommandHandler : ICommandHandler<UpdateGarmentAvalProductCommand, GarmentAvalProduct>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentAvalProductCommandHandler(IStorage storage)
        {
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentAvalProduct> Handle(UpdateGarmentAvalProductCommand request, CancellationToken cancellaitonToken)
        {
            var garmentAvalProduct = _garmentAvalProductRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentAvalProduct == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            garmentAvalProduct.SetRONo(request.RONo);
            garmentAvalProduct.SetArticle(request.Article);
            garmentAvalProduct.SetAvalDate(request.AvalDate);

            var dbGarmentAvalProduct = _garmentAvalProductItemRepository.Find(y => y.APId == garmentAvalProduct.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentAvalProduct.Any(y => y.APId == garmentAvalProduct.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentAvalProduct.Any(y => y.APId == garmentAvalProduct.Identity));
            var deletedItems = dbGarmentAvalProduct.Where(x => !request.Items.Any(y => y.APId == garmentAvalProduct.Identity));

            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentAvalProduct.Find(x => x.Identity == item.Identity);
                dbItem.setPreparingId(item.PreparingId);
                dbItem.setPreparingItemId(item.PreparingItemId);
                dbItem.setProductId(new ProductId(item.Product.Id));
                dbItem.setProductCode(item.Product.Code);
                dbItem.setProductName(item.Product.Name);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setQuantity(item.Quantity);
                dbItem.setUomId(new UomId(item.Uom.Id));
                dbItem.setUomUnit(item.Uom.Unit);
                await _garmentAvalProductItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentAvalProductItem(Guid.NewGuid(), garmentAvalProduct.Identity, x.PreparingId, x.PreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.BasicPrice, x.IsReceived)).ToList()
                .ForEach(async x => await _garmentAvalProductItemRepository.Update(x));

            foreach (var item in deletedItems)
            {
                item.SetDeleted();
                await _garmentAvalProductItemRepository.Update(item);
            }


            garmentAvalProduct.SetModified();

            await _garmentAvalProductRepository.Update(garmentAvalProduct);

            _storage.Save();

            return garmentAvalProduct;
        }
    }
}