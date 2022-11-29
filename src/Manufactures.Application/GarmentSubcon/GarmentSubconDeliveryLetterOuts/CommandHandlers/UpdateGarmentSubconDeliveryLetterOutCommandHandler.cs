using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<UpdateGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentServiceSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentServiceSubconSewingRepository _garmentSubconSewingRepository;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = _storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(UpdateGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            if(subconDeliveryLetterOut.SubconCategory == "SUBCON CUTTING SEWING")
            {

                //subconDeliveryLetterOut.SetEPOItemId(request.EPOItemId);
                //subconDeliveryLetterOut.SetPONo(request.PONo);

                _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
                {
                    var item = request.Items.Where(o => o.Id == subconDeliveryLetterOutItem.Identity).Single();

                    subconDeliveryLetterOutItem.SetQuantity(item.Quantity);

                    subconDeliveryLetterOutItem.Modify();

                    await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
                });
            }
            else
            {
                _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDLItem =>
                {
                    var item = request.Items.Where(o => o.Id == subconDLItem.Identity).SingleOrDefault();
                    
                    if (item==null)
                    {
                        if (subconDeliveryLetterOut.SubconCategory == "SUBCON SEWING")
                        {
                            var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                            subconCuttingOut.SetIsUsed(false);
                            subconCuttingOut.Modify();

                            await _garmentCuttingOutRepository.Update(subconCuttingOut);
                        }
                        else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN")
                        {
                            var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                            subconCutting.SetIsUsed(false);
                            subconCutting.Modify();

                            await _garmentSubconCuttingRepository.Update(subconCutting);
                        }
                        else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA GARMENT WASH")
                        {
                            var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                            subconSewing.SetIsUsed(false);
                            subconSewing.Modify();

                            await _garmentSubconSewingRepository.Update(subconSewing);
                        }
                        else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                        {
                            var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                            subconPanel.SetIsUsed(false);
                            subconPanel.Modify();

                            await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                        }
                        else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                        {
                            var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDLItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                            subconFabric.SetIsUsed(false);
                            subconFabric.Modify();

                            await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                        }
                        subconDLItem.Remove();
                    }
                    else
                    {
                        subconDLItem.Modify();
                    }


                    await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDLItem);
                });

                foreach(var item in request.Items)
                {
                    if (item.Id == Guid.Empty)
                    {
                        GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(
                            Guid.NewGuid(),
                            subconDeliveryLetterOut.Identity,
                            item.UENItemId,
                            new ProductId(item.Product.Id),
                            item.Product.Code,
                            item.Product.Name,
                            item.ProductRemark,
                            item.DesignColor,
                            item.Quantity,
                            new UomId(item.Uom.Id),
                            item.Uom.Unit,
                            new UomId(item.UomOut.Id),
                            item.UomOut.Unit,
                            item.FabricType,
                            item.SubconId,
                            item.RONo,
                            item.POSerialNumber,
                            item.SubconNo,
                            item.QtyPacking,
                            item.UomSatuanUnit
                        );
                        if (request.SubconCategory == "SUBCON SEWING")
                        {
                            var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                            subconCuttingOut.SetIsUsed(true);
                            subconCuttingOut.Modify();

                            await _garmentCuttingOutRepository.Update(subconCuttingOut);
                        }
                        else if (request.SubconCategory == "SUBCON JASA KOMPONEN")
                        {
                            var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                            subconCutting.SetIsUsed(true);
                            subconCutting.Modify();

                            await _garmentSubconCuttingRepository.Update(subconCutting);
                        }
                        else if (request.SubconCategory == "SUBCON JASA GARMENT WASH")
                        {
                            var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                            subconSewing.SetIsUsed(true);
                            subconSewing.Modify();

                            await _garmentSubconSewingRepository.Update(subconSewing);
                        }
                        else if (request.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                        {
                            var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                            subconPanel.SetIsUsed(true);
                            subconPanel.Modify();

                            await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                        }
                        else if (request.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                        {
                            var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                            subconFabric.SetIsUsed(true);
                            subconFabric.Modify();

                            await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                        }
                        await _garmentSubconDeliveryLetterOutItemRepository.Update(garmentSubconDeliveryLetterOutItem);
                    }
                }
            }

            subconDeliveryLetterOut.SetDate(request.DLDate.GetValueOrDefault());
            subconDeliveryLetterOut.SetRemark(request.Remark);

            subconDeliveryLetterOut.Modify();

            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
