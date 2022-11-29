using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class RemoveGarmentSampleAvalProductCommandHandler : ICommandHandler<RemoveGarmentSampleAvalProductCommand, GarmentSampleAvalProduct>
    {
        private readonly IGarmentSampleAvalProductRepository _garmentSampleAvalProductRepository;
        private readonly IGarmentSampleAvalProductItemRepository _garmentSampleAvalProductItemRepository;
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentSampleAvalProductCommandHandler(IStorage storage)
        {
            _garmentSampleAvalProductRepository = storage.GetRepository<IGarmentSampleAvalProductRepository>();
            _garmentSampleAvalProductItemRepository = storage.GetRepository<IGarmentSampleAvalProductItemRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSampleAvalProduct> Handle(RemoveGarmentSampleAvalProductCommand request, CancellationToken cancellationToken)
        {
            var garmentSampleAvalProduct = _garmentSampleAvalProductRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentSampleAvalProduct == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentSampleAvalProductItems = _garmentSampleAvalProductItemRepository.Find(x => x.APId == request.Id);

            foreach (var item in garmentSampleAvalProductItems)
            {
                item.Remove();
                await _garmentSampleAvalProductItemRepository.Update(item);

                var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Find(o => o.Identity == Guid.Parse(item.SamplePreparingItemId.Value)).Single();

                garmentSamplePreparingItem.setRemainingQuantityZeroValue(garmentSamplePreparingItem.RemainingQuantity + item.Quantity);

                garmentSamplePreparingItem.SetModified();
                await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
            }

            garmentSampleAvalProduct.Remove();

            await _garmentSampleAvalProductRepository.Update(garmentSampleAvalProduct);

            _storage.Save();

            return garmentSampleAvalProduct;
        }
    }
}
