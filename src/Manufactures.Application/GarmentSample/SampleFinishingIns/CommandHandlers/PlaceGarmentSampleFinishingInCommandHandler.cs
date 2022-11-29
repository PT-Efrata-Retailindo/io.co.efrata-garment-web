using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers
{
    public class PlaceGarmentSampleFinishingInCommandHandler : ICommandHandler<PlaceGarmentSampleFinishingInCommand, GarmentSampleFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;

        public PlaceGarmentSampleFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
        }

        public async Task<GarmentSampleFinishingIn> Handle(PlaceGarmentSampleFinishingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSampleFinishingIn garmentFinishingIn = new GarmentSampleFinishingIn(
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
                GarmentSampleFinishingInItem garmentFinishingInItem = new GarmentSampleFinishingInItem(
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
                    item.Price,
                    0
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
                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSampleSewingOutItem(s)).Single();
                garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity - sewingDOItem.Value);
                garmentSewingOutItem.Modify();

                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
            }

            await _garmentFinishingInRepository.Update(garmentFinishingIn);

            _storage.Save();

            return garmentFinishingIn;
        }

        private string GenerateFinishingInNo(PlaceGarmentSampleFinishingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var prefix = $"FIS{unitcode}{year}{month}";

            var lastFinishingInNo = _garmentFinishingInRepository.Query.Where(w => w.FinishingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingInNo)
                .Select(s => int.Parse(s.FinishingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastFinishingInNo + 1).ToString("D5")}";

            return finInNo;
        }
    }
}
