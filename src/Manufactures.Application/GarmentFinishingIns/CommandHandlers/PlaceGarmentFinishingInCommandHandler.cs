using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSewingOuts;

namespace Manufactures.Application.GarmentFinishingIns.CommandHandlers
{
    public class PlaceGarmentFinishingInCommandHandler : ICommandHandler<PlaceGarmentFinishingInCommand, GarmentFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;

        public PlaceGarmentFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
        }

        public async Task<GarmentFinishingIn> Handle(PlaceGarmentFinishingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentFinishingIn garmentFinishingIn = new GarmentFinishingIn(
                Guid.NewGuid(),
                GenerateFinishingInNo(request),
                request.FinishingInType,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.FinishingInDate.GetValueOrDefault(),
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.DOId,
                request.DONo,
                request.SubconType
            );

            Dictionary<Guid, double> sewingOutItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                    Guid.NewGuid(),
                    garmentFinishingIn.Identity,
                    item.SewingOutItemId,
                    item.SewingOutDetailId,
                    Guid.Empty,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.Quantity,
                    item.RemainingQuantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Color,
                    item.BasicPrice,
                    item.Price
                );

                if (sewingOutItemToBeUpdated.ContainsKey(item.SewingOutItemId))
                {
                    sewingOutItemToBeUpdated[item.SewingOutItemId] += item.Quantity;
                }
                else
                {
                    sewingOutItemToBeUpdated.Add(item.SewingOutItemId, item.Quantity);
                }

                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                
            }

            foreach (var sewingDOItem in sewingOutItemToBeUpdated)
            {
                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingOutItem(s)).Single();
                garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity - sewingDOItem.Value);
                garmentSewingOutItem.Modify();

                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
            }

            await _garmentFinishingInRepository.Update(garmentFinishingIn);

            _storage.Save();

            return garmentFinishingIn;
        }

        private string GenerateFinishingInNo(PlaceGarmentFinishingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"FI{unitcode}{year}{month}";

            var lastFinishingInNo = _garmentFinishingInRepository.Query.Where(w => w.FinishingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingInNo)
                .Select(s => int.Parse(s.FinishingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastFinishingInNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}
