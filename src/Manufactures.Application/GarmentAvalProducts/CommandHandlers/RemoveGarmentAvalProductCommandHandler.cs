using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalProducts.CommandHandlers
{
    public class RemoveGarmentAvalProductCommandHandler : ICommandHandler<RemoveGarmentAvalProductCommand, GarmentAvalProduct>
    {
        private readonly IGarmentAvalProductRepository _garmentAvalProductRepository;
        private readonly IGarmentAvalProductItemRepository _garmentAvalProductItemRepository;
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentAvalProductCommandHandler(IStorage storage)
        {
            _garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
            _garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentAvalProduct> Handle(RemoveGarmentAvalProductCommand request, CancellationToken cancellationToken)
        {
            var garmentAvalProduct = _garmentAvalProductRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentAvalProduct == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentAvalProductItems = _garmentAvalProductItemRepository.Find(x => x.APId == request.Id);

            foreach (var item in garmentAvalProductItems)
            {
                item.Remove();
                await _garmentAvalProductItemRepository.Update(item);

                var garmentPreparingItem = _garmentPreparingItemRepository.Find(o => o.Identity == Guid.Parse(item.PreparingItemId.Value)).Single();

                garmentPreparingItem.setRemainingQuantityZeroValue(garmentPreparingItem.RemainingQuantity + item.Quantity);

                garmentPreparingItem.SetModified();
                await _garmentPreparingItemRepository.Update(garmentPreparingItem);
            }

            garmentAvalProduct.Remove();

            await _garmentAvalProductRepository.Update(garmentAvalProduct);

            _storage.Save();

            return garmentAvalProduct;
        }
    }
}