using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class UpdateGarmentSampleAvalProductCommandHandler : ICommandHandler<UpdateGarmentSampleAvalProductCommand, GarmentSampleAvalProduct>
    {
        private readonly IGarmentSampleAvalProductRepository _garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository _garmentSampleAvalProductItemRepository;
        private readonly IStorage _storage;

        public UpdateGarmentSampleAvalProductCommandHandler(IStorage storage)
        {
            _garmentSampleAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            _garmentSampleAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSampleAvalProduct> Handle(UpdateGarmentSampleAvalProductCommand request, CancellationToken cancellaitonToken)
        {
            var garmentSampleAvalProduct = _garmentSampleAvalProductRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentSampleAvalProduct == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            garmentSampleAvalProduct.SetRONo(request.RONo);
            garmentSampleAvalProduct.SetArticle(request.Article);
            garmentSampleAvalProduct.SetAvalDate(request.AvalDate);

            var dbGarmentAvalProduct = _garmentSampleAvalProductItemRepository.Find(y => y.APId == garmentSampleAvalProduct.Identity);
            var updatedItems = request.Items.Where(x => dbGarmentAvalProduct.Any(y => y.APId == garmentSampleAvalProduct.Identity));
            var addedItems = request.Items.Where(x => !dbGarmentAvalProduct.Any(y => y.APId == garmentSampleAvalProduct.Identity));
            var deletedItems = dbGarmentAvalProduct.Where(x => !request.Items.Any(y => y.APId == garmentSampleAvalProduct.Identity));

            foreach (var item in updatedItems)
            {
                var dbItem = dbGarmentAvalProduct.Find(x => x.Identity == item.Identity);
                dbItem.setSamplePreparingId(item.SamplePreparingId);
                dbItem.setSamplePreparingItemId(item.SamplePreparingItemId);
                dbItem.setProductId(new ProductId(item.Product.Id));
                dbItem.setProductCode(item.Product.Code);
                dbItem.setProductName(item.Product.Name);
                dbItem.setDesignColor(item.DesignColor);
                dbItem.setQuantity(item.Quantity);
                dbItem.setUomId(new UomId(item.Uom.Id));
                dbItem.setUomUnit(item.Uom.Unit);
                await _garmentSampleAvalProductItemRepository.Update(dbItem);
            }

            addedItems.Select(x => new GarmentSampleAvalProductItem(Guid.NewGuid(), garmentSampleAvalProduct.Identity, x.SamplePreparingId, x.SamplePreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.BasicPrice, x.IsReceived)).ToList()
                .ForEach(async x => await _garmentSampleAvalProductItemRepository.Update(x));

            foreach (var item in deletedItems)
            {
                item.SetDeleted();
                await _garmentSampleAvalProductItemRepository.Update(item);
            }


            garmentSampleAvalProduct.SetModified();

            await _garmentSampleAvalProductRepository.Update(garmentSampleAvalProduct);

            _storage.Save();

            return garmentSampleAvalProduct;
        }
    }
}
