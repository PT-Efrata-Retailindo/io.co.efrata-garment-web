using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.CommandHandlers
{
    public class PlaceGarmentServiceSubconShrinkagePanelCommandHandler : ICommandHandler<PlaceGarmentServiceSubconShrinkagePanelCommand, GarmentServiceSubconShrinkagePanel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconShrinkagePanelItemRepository _garmentServiceSubconShrinkagePanelItemRepository;
        private readonly IGarmentServiceSubconShrinkagePanelDetailRepository _garmentServiceSubconShrinkagePanelDetailRepository;

        public PlaceGarmentServiceSubconShrinkagePanelCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconShrinkagePanelItemRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _garmentServiceSubconShrinkagePanelDetailRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelDetailRepository>();
        }

        public async Task<GarmentServiceSubconShrinkagePanel> Handle(PlaceGarmentServiceSubconShrinkagePanelCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentServiceSubconShrinkagePanel garmentServiceSubconShrinkagePanel = new GarmentServiceSubconShrinkagePanel(
                Guid.NewGuid(),
                GenerateServiceSubconShrinkagePanelNo(request),
                request.ServiceSubconShrinkagePanelDate.GetValueOrDefault(),
                request.Remark,
                request.IsUsed,
                request.QtyPacking,
                request.UomUnit
            );

            foreach (var item in request.Items)
            {
                GarmentServiceSubconShrinkagePanelItem garmentServiceSubconShrinkagePanelItem = new GarmentServiceSubconShrinkagePanelItem(
                    Guid.NewGuid(),
                    garmentServiceSubconShrinkagePanel.Identity,
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
                        GarmentServiceSubconShrinkagePanelDetail garmentServiceSubconShrinkagePanelDetail = new GarmentServiceSubconShrinkagePanelDetail(
                                     Guid.NewGuid(),
                                     garmentServiceSubconShrinkagePanelItem.Identity,
                                     new ProductId(detail.Product.Id),
                                     detail.Product.Code,
                                     detail.Product.Name,
                                     detail.Product.Remark,
                                     detail.DesignColor,
                                     detail.Quantity,
                                     new UomId(detail.Uom.Id),
                                     detail.Uom.Unit
                                 );
                        await _garmentServiceSubconShrinkagePanelDetailRepository.Update(garmentServiceSubconShrinkagePanelDetail);
                    }
                }
                await _garmentServiceSubconShrinkagePanelItemRepository.Update(garmentServiceSubconShrinkagePanelItem);
            }

            await _garmentServiceSubconShrinkagePanelRepository.Update(garmentServiceSubconShrinkagePanel);

            _storage.Save();

            return garmentServiceSubconShrinkagePanel;
        }

        private string GenerateServiceSubconShrinkagePanelNo(PlaceGarmentServiceSubconShrinkagePanelCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SJSK{year}{month}";

            var lastServiceSubconShrinkagePanelNo = _garmentServiceSubconShrinkagePanelRepository.Query.Where(w => w.ServiceSubconShrinkagePanelNo.StartsWith(prefix))
                .OrderByDescending(o => o.ServiceSubconShrinkagePanelNo)
                .Select(s => int.Parse(s.ServiceSubconShrinkagePanelNo.Replace(prefix, "")))
                .FirstOrDefault();
            var ServiceSubconShrinkagePanelNo = $"{prefix}{(lastServiceSubconShrinkagePanelNo + 1).ToString("D4")}";

            return ServiceSubconShrinkagePanelNo;
        }
    }
}
