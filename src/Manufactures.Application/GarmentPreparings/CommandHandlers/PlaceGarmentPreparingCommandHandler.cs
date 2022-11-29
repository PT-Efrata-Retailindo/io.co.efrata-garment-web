using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentPreparings.CommandHandlers
{
    public class PlaceGarmentPreparingCommandHandler : ICommandHandler<PlaceGarmentPreparingCommand, GarmentPreparing>
    {
        private readonly IGarmentPreparingRepository _garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;
        private readonly IStorage _storage;

        public PlaceGarmentPreparingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
        }

        public async Task<GarmentPreparing> Handle(PlaceGarmentPreparingCommand request, CancellationToken cancellationToken)
        {
            //var garmentPreparing  = _garmentPreparingRepository.Find(o =>
            //                        o.UENId == request.UENId &&
            //                        o.UENNo == request.UENNo &&
            //                        o.UnitId == request.Unit.Id &&
            //                        o.UnitCode == request.Unit.Code &&
            //                        o.ProcessDate == request.ProcessDate &&
            //                        o.RONo == request.RONo &&
            //                        o.Article == request.Article &&
            //                        o.IsCuttingIn == request.IsCuttingIn).FirstOrDefault();
            //List<GarmentPreparingItem> garmentPreparingItem = new List<GarmentPreparingItem>();
            //if (garmentPreparing == null)
            //{
                var garmentPreparing = new GarmentPreparing(Guid.NewGuid(), request.UENId, request.UENNo, new UnitDepartmentId(request.Unit.Id), request.Unit.Code, request.Unit.Name, request.ProcessDate, request.RONo,
                        request.Article, request.IsCuttingIn,new Domain.Shared.ValueObjects.BuyerId( request.Buyer.Id), request.Buyer.Code, request.Buyer.Name);
                request.Items.Select(x => new GarmentPreparingItem(Guid.NewGuid(), x.UENItemId, new ProductId(x.Product.Id), x.Product.Code, x.Product.Name, x.DesignColor, x.Quantity, new UomId(x.Uom.Id), x.Uom.Unit, x.FabricType, x.RemainingQuantity, x.BasicPrice, garmentPreparing.Identity,x.ROSource,x.CustomsCategory)).ToList()
                    .ForEach(async x => await _garmentPreparingItemRepository.Update(x));
            //}

            garmentPreparing.SetModified();

            await _garmentPreparingRepository.Update(garmentPreparing);

            _storage.Save();

            return garmentPreparing;

        }
    }
}
