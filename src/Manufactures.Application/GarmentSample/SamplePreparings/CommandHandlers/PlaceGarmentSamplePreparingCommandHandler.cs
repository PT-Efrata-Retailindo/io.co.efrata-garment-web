using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers
{
    public class PlaceGarmentSamplePreparingCommandHandler : ICommandHandler<PlaceGarmentSamplePreparingCommand, GarmentSamplePreparing>
    {
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentSamplePreparingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
        }

        public async Task<GarmentSamplePreparing> Handle(PlaceGarmentSamplePreparingCommand request, CancellationToken cancellationToken)
        {
            var garmentSamplePreparing = new GarmentSamplePreparing(Guid.NewGuid(), request.UENId, request.UENNo, new UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name, request.ProcessDate, request.RONo,
                    request.Article, request.IsCuttingIn, new Domain.Shared.ValueObjects.BuyerId(request.Buyer.Id), request.Buyer.Code, request.Buyer.Name);
            request.Items.Select(x => new GarmentSamplePreparingItem(Guid.NewGuid(), x.UENItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.FabricType, x.RemainingQuantity, x.BasicPrice, garmentSamplePreparing.Identity, x.ROSource)).ToList()
                .ForEach(async x => await _garmentSamplePreparingItemRepository.Update(x));

            garmentSamplePreparing.SetModified();

            await _garmentSamplePreparingRepository.Update(garmentSamplePreparing);

            _storage.Save();

            return garmentSamplePreparing;
        }
    }
}
