using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using System.Threading;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class PlaceGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<PlaceGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;
        private readonly IGarmentSubconDeliveryLetterOutItemRepository _garmentSubconDeliveryLetterOutItemRepository;
        private readonly IGarmentSubconCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentServiceSubconCuttingRepository _garmentSubconCuttingRepository;
        private readonly IGarmentServiceSubconSewingRepository _garmentSubconSewingRepository;
        private readonly IGarmentServiceSubconShrinkagePanelRepository _garmentServiceSubconShrinkagePanelRepository;
        private readonly IGarmentServiceSubconFabricWashRepository _garmentServiceSubconFabricWashRepository;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;

        public PlaceGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingRepository= storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentSubconSewingRepository=storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
        }

        public async Task<GarmentSubconDeliveryLetterOut> Handle(PlaceGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentSubconDeliveryLetterOut garmentSubconDeliveryLetterOut = new GarmentSubconDeliveryLetterOut(
                Guid.NewGuid(),
                GenerateNo(request),
                request.DLType,
                request.ContractType,
                request.DLDate,
                request.UENId,
                request.UENNo,
                request.PONo,
                request.EPOItemId,
                request.Remark,
                request.IsUsed,
                request.ServiceType,
                request.SubconCategory,
                request.EPOId,
                request.EPONo,
                request.QtyPacking,
                request.UomUnit
            );

            foreach (var item in request.Items)
            {
                GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(
                    Guid.NewGuid(),
                    garmentSubconDeliveryLetterOut.Identity,
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
                if(request.SubconCategory=="SUBCON SEWING")
                {
                    var subconCuttingOut= _garmentCuttingOutRepository.Query.Where(x => x.Identity == item.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    subconCuttingOut.SetIsUsed(true);
                    subconCuttingOut.Modify();

                    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                }
                else if(request.SubconCategory == "SUBCON JASA KOMPONEN")
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

            //var subconContract = _garmentSubconContractRepository.Query.Where(x => x.Identity == garmentSubconDeliveryLetterOut.SubconContractId).Select(s => new GarmentSubconContract(s)).Single();
            //subconContract.SetIsUsed(true);
            //subconContract.Modify();

            //await _garmentSubconContractRepository.Update(subconContract);

            await _garmentSubconDeliveryLetterOutRepository.Update(garmentSubconDeliveryLetterOut);

            _storage.Save();
            return garmentSubconDeliveryLetterOut;
        }

        private string GenerateNo(PlaceGarmentSubconDeliveryLetterOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.DLType == "PROSES" ? "" : "/R";
            var type = request.ContractType == "SUBCON BAHAN BAKU" ? "BB" : request.ContractType == "SUBCON CUTTING" ? "CT" : request.ContractType == "SUBCON GARMENT" ? "SG":"JS";

            var prefix = $"SJK/{type}/{year}{month}";

            var lastNo = _garmentSubconDeliveryLetterOutRepository.Query.Where(w => w.DLNo.StartsWith(prefix))
                .OrderByDescending(o => o.DLNo)
                .Select(s => int.Parse(s.DLNo.Substring(11, 4)))
                .FirstOrDefault();
            var no = $"{prefix}{(lastNo + 1).ToString("D4")}{code}";

            return no;
        }
    }
}