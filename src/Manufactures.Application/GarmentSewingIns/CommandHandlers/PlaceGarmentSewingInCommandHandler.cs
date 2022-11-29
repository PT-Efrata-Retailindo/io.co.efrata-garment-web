using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingIns.CommandHandlers
{
    public class PlaceGarmentSewingInCommandHandler : ICommandHandler<PlaceGarmentSewingInCommand, GarmentSewingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;

        public PlaceGarmentSewingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            //_garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
        }

        public async Task<GarmentSewingIn> Handle(PlaceGarmentSewingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                Guid.NewGuid(),
                GenerateSewingInNo(request),
                request.SewingFrom,
                request.LoadingId,
                request.LoadingNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SewingInDate.GetValueOrDefault()
            );

            foreach (var item in request.Items)
            {
                GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                    Guid.NewGuid(),
                    garmentSewingIn.Identity,
                    item.SewingOutItemId,
                    item.SewingOutDetailId,
                    item.LoadingItemId,
                    item.FinishingOutItemId,
                    item.FinishingOutDetailId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    item.Quantity,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Color,
                    item.Quantity,
                    item.BasicPrice,
                    item.Price
                );

                if (request.SewingFrom == "CUTTING")
                {
                    var garmentLoadingItem = _garmentLoadingItemRepository.Query.Where(o => o.Identity == item.LoadingItemId).Select(s => new GarmentLoadingItem(s)).Single();

                    garmentLoadingItem.SetRemainingQuantity(garmentLoadingItem.RemainingQuantity - item.Quantity);

                    garmentLoadingItem.Modify();
                    await _garmentLoadingItemRepository.Update(garmentLoadingItem);
                }
                else if(request.SewingFrom == "SEWING")
                {
                    var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(s => s.Identity == item.SewingOutItemId).Select(s => new GarmentSewingOutItem(s)).Single();

                    garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity - item.Quantity);

                    garmentSewingOutItem.Modify();
                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }
                else if (request.SewingFrom == "FINISHING")
                {
                    var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(s => s.Identity == item.FinishingOutItemId).Select(s => new GarmentFinishingOutItem(s)).Single();

                    garmentFinishingOutItem.SetRemainingQuantity(garmentFinishingOutItem.RemainingQuantity - item.Quantity);

                    garmentFinishingOutItem.Modify();
                    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                }

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }

        private string GenerateSewingInNo(PlaceGarmentSewingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.Unit.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }
    }
}