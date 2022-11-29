using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.CommandHandlers
{
    public class PlaceGarmentServiceSubconFabricWashCommandHandler : ICommandHandler<PlaceGarmentServiceSubconFabricWashCommand, GarmentServiceSubconFabricWash>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentServiceSubconFabricWashItemRepository _garmentServiceSubconFabricWashItemRepository;
        private readonly IGarmentServiceSubconFabricWashDetailRepository _garmentServiceSubconFabricWashDetailRepository;

        public PlaceGarmentServiceSubconFabricWashCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentServiceSubconFabricWashItemRepository = storage.GetRepository<IGarmentServiceSubconFabricWashItemRepository>();
            _garmentServiceSubconFabricWashDetailRepository = storage.GetRepository<IGarmentServiceSubconFabricWashDetailRepository>();
        }

        public async Task<GarmentServiceSubconFabricWash> Handle(PlaceGarmentServiceSubconFabricWashCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSubconFabricWash garmentServiceSubconFabricWash = new GarmentServiceSubconFabricWash(
                Guid.NewGuid(),
                GenerateServiceSubconFabricWashNo(request),
                request.ServiceSubconFabricWashDate.GetValueOrDefault(),
                request.Remark,
                request.IsUsed,
                request.QtyPacking,
                request.UomUnit
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconFabricWashItem garmentServiceSubconFabricWashItem = new GarmentServiceSubconFabricWashItem(
                    Guid.NewGuid(),
                    garmentServiceSubconFabricWash.Identity,
                    item.UnitExpenditureNo,
                    item.ExpenditureDate,
                    new UnitSenderId(item.UnitSender.Id),
                    item.UnitSender.Code,
                    item.UnitSender.Name,
                    new UnitRequestId(item.UnitRequest.Id),
                    item.UnitRequest.Code,
                    item.UnitRequest.Name
                );

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentServiceSubconFabricWashDetail garmentServiceSubconFabricWashDetail = new GarmentServiceSubconFabricWashDetail(
                                     Guid.NewGuid(),
                                     garmentServiceSubconFabricWashItem.Identity,
                                     new ProductId(detail.Product.Id),
                                     detail.Product.Code,
                                     detail.Product.Name,
                                     detail.Product.Remark,
                                     detail.DesignColor,
                                     detail.Quantity,
                                     new UomId(detail.Uom.Id),
                                     detail.Uom.Unit
                                 );
                        await _garmentServiceSubconFabricWashDetailRepository.Update(garmentServiceSubconFabricWashDetail);
                    }
                }
                await _garmentServiceSubconFabricWashItemRepository.Update(garmentServiceSubconFabricWashItem);
            }

            await _garmentServiceSubconFabricWashRepository.Update(garmentServiceSubconFabricWash);

            _storage.Save();

            return garmentServiceSubconFabricWash;
        }

        private string GenerateServiceSubconFabricWashNo(PlaceGarmentServiceSubconFabricWashCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJF{year}{month}";

            var lastServiceSubconFabricWashNo = _garmentServiceSubconFabricWashRepository.Query.Where(w => w.ServiceSubconFabricWashNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSubconFabricWashNo)
                .Select(s => int.Parse(s.ServiceSubconFabricWashNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSubconFabricWashNo = $"{prefix}{(lastServiceSubconFabricWashNo + 1).ToString("D4")}";

            return ServiceSubconFabricWashNo;
        }
    }
}
