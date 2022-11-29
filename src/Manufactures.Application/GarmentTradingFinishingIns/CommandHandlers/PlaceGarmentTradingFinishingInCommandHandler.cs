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
//using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentTradingFinishingIns.Commands;
//using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
//using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentComodityPrices.Repositories;

namespace Manufactures.Application.GarmentFinishingIns.CommandHandlers
{
    public class PlaceGarmentTradingFinishingInCommandHandler : ICommandHandler<PlaceGarmentTradingFinishingInCommand, GarmentFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        //private readonly IGarmentSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentTradingFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            //_garmentSubconCuttingRepository = storage.GetRepository<IGarmentSubconCuttingRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentFinishingIn> Handle(PlaceGarmentTradingFinishingInCommand request, CancellationToken cancellationToken)
        {
            var no = GenerateFinishingInNo();

            GarmentFinishingIn garmentFinishingIn = new GarmentFinishingIn(
                Guid.NewGuid(),
                no,
                request.FinishingInType,
                new UnitDepartmentId(0),
                null,
                null,
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

            var comodityPrice = _garmentComodityPriceRepository
                .Query
                .OrderByDescending(o => o.CreatedDate)
                .Where(c => c.UnitId == request.Unit.Id && c.ComodityId == request.Comodity.Id)
                .Select(c => c.Price)
                .FirstOrDefault();

            //Dictionary<Guid, double> subconCuttingSumQuantities = new Dictionary<Guid, double>();

            foreach (var item in request.Items.Where(i => i.IsSave))
            {
                GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                    Guid.NewGuid(),
                    garmentFinishingIn.Identity,
                    Guid.Empty,
                    Guid.Empty,
                    item.SubconCuttingId,
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
                    (double)(((decimal)item.BasicPrice + comodityPrice * (decimal)0.75) * (decimal)item.Quantity)
                );

                //if (Guid.Empty != item.SubconCuttingId)
                //{
                //    subconCuttingSumQuantities[item.SubconCuttingId] = subconCuttingSumQuantities.GetValueOrDefault(item.SubconCuttingId) + item.Quantity;
                //}

                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
            }

            //foreach (var sumQuantity in subconCuttingSumQuantities)
            //{
            //    var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == sumQuantity.Key).Select(s => new GarmentSubconCutting(s)).Single();
            //    subconCutting.SetFinishingInQuantity(subconCutting.FinishingInQuantity + sumQuantity.Value);
            //    subconCutting.Modify();

            //    await _garmentSubconCuttingRepository.Update(subconCutting);
            //}

            await _garmentFinishingInRepository.Update(garmentFinishingIn);

            _storage.Save();

            return garmentFinishingIn;
        }

        private string GenerateFinishingInNo()
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"FT{year}{month}";

            var lastNo = _garmentFinishingInRepository.Query.Where(w => w.FinishingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingInNo)
                .Select(s => int.Parse(s.FinishingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}
