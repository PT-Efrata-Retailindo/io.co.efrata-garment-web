using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingDOs.CommandHandlers
{
    public class PlaceGarmentSewingDOCommandHandler: ICommandHandler<PlaceGarmentSewingDOCommand, GarmentSewingDO>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        //private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;

        public PlaceGarmentSewingDOCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            //_garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
        }

        public async Task<GarmentSewingDO> Handle(PlaceGarmentSewingDOCommand request, CancellationToken cancellationToken)
        {
            //request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentSewingDO garmentSewingDO = new GarmentSewingDO(
                Guid.NewGuid(),
                GenerateSewingDONo(request),
                request.CuttingOutId,
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
                request.SewingDODate.GetValueOrDefault()
            );

            foreach (var item in request.Items)
            {
                GarmentSewingDOItem garmentSewingDOItem = new GarmentSewingDOItem(
                    Guid.NewGuid(),
                    garmentSewingDO.Identity,
                    item.CuttingOutDetailId,
                    item.CuttingOutItemId,
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
                    item.RemainingQuantity,
                    item.BasicPrice,
                    item.Price
                );
                await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
            }

            await _garmentSewingDORepository.Update(garmentSewingDO);

            _storage.Save();

            return garmentSewingDO;
        }

        private string GenerateSewingDONo(PlaceGarmentSewingDOCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"DS{request.Unit.Code}{year}{month}";

            var lastSewingDONo = _garmentSewingDORepository.Query.Where(w => w.SewingDONo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingDONo)
                .Select(s => int.Parse(s.SewingDONo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingDONo = $"{prefix}{(lastSewingDONo + 1).ToString("D4")}";

            return SewingDONo;
        }
    }
}