using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class PlaceGarmentSampleAvalProductCommandHandler : ICommandHandler<PlaceGarmentSampleAvalProductCommand, GarmentSampleAvalProduct>
    {
        private readonly IGarmentSampleAvalProductRepository _garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository _garmentSampleAvalProductItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentSampleAvalProductCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            _garmentSampleAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleAvalProduct> Handle(PlaceGarmentSampleAvalProductCommand request, CancellationToken cancellationToken)
        {
            //var garmentAvalProduct = _garmentAvalProductRepository.Find(o =>
            //                       o.RONo == request.RONo &&
            //                       o.Article == request.Article &&
            //                       o.AvalDate == request.AvalDate).FirstOrDefault();
            //List<GarmentAvalProductItem> garmentAvalProductItem = new List<GarmentAvalProductItem>();
            //if (garmentAvalProduct == null)
            //{
            GarmentSampleAvalProduct garmentSampleAvalProduct = new GarmentSampleAvalProduct(Guid.NewGuid(), request.RONo, request.Article, request.AvalDate, new Domain.Shared.ValueObjects.UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name);
            request.Items.Select(x => new GarmentSampleAvalProductItem(Guid.NewGuid(), garmentSampleAvalProduct.Identity, x.SamplePreparingId, x.SamplePreparingItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.BasicPrice, x.IsReceived)).ToList()
                .ForEach(async x => await _garmentSampleAvalProductItemRepository.Update(x));
            //}

            foreach (var itemPreparing in request.Items)
            {
                var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Find(o => o.Identity == Guid.Parse(itemPreparing.SamplePreparingItemId.Value)).Single();

                garmentSamplePreparingItem.setRemainingQuantityZeroValue(garmentSamplePreparingItem.RemainingQuantity - itemPreparing.Quantity);

                garmentSamplePreparingItem.SetModified();
                await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
            }

            garmentSampleAvalProduct.SetModified();

            await _garmentSampleAvalProductRepository.Update(garmentSampleAvalProduct);

            _storage.Save();

            return garmentSampleAvalProduct;

        }
    }
}
