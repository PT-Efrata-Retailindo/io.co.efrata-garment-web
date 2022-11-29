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
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers
{
    public class RemoveGarmentSubconDeliveryLetterOutCommandHandler : ICommandHandler<RemoveGarmentSubconDeliveryLetterOutCommand, GarmentSubconDeliveryLetterOut>
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

        public RemoveGarmentSubconDeliveryLetterOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
            _garmentSubconDeliveryLetterOutItemRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutItemRepository>();
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSubconCuttingOutRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentServiceSubconCuttingRepository>();
            _garmentSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconShrinkagePanelRepository = storage.GetRepository<IGarmentServiceSubconShrinkagePanelRepository>();
            _garmentServiceSubconFabricWashRepository = storage.GetRepository<IGarmentServiceSubconFabricWashRepository>();
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
        }


        public async Task<GarmentSubconDeliveryLetterOut> Handle(RemoveGarmentSubconDeliveryLetterOutCommand request, CancellationToken cancellationToken)
        {
            var subconDeliveryLetterOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconDeliveryLetterOut(o)).Single();

            _garmentSubconDeliveryLetterOutItemRepository.Find(o => o.SubconDeliveryLetterOutId == subconDeliveryLetterOut.Identity).ForEach(async subconDeliveryLetterOutItem =>
            {
                subconDeliveryLetterOutItem.Remove();
                if (subconDeliveryLetterOut.SubconCategory == "SUBCON SEWING")
                {
                    var subconCuttingOut = _garmentCuttingOutRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentSubconCuttingOut(s)).Single();
                    subconCuttingOut.SetIsUsed(false);
                    subconCuttingOut.Modify();

                    await _garmentCuttingOutRepository.Update(subconCuttingOut);
                }
                else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA KOMPONEN")
                {
                    var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconCutting(s)).Single();
                    subconCutting.SetIsUsed(false);
                    subconCutting.Modify();

                    await _garmentSubconCuttingRepository.Update(subconCutting);
                }
                else if (subconDeliveryLetterOut.SubconCategory == "SUBCON JASA GARMENT WASH")
                {
                    var subconSewing = _garmentSubconSewingRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconSewing(s)).Single();
                    subconSewing.SetIsUsed(false);
                    subconSewing.Modify();

                    await _garmentSubconSewingRepository.Update(subconSewing);
                }
                else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB SHRINKAGE/PANEL")
                {
                    var subconPanel = _garmentServiceSubconShrinkagePanelRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconShrinkagePanel(s)).Single();
                    subconPanel.SetIsUsed(false);
                    subconPanel.Modify();

                    await _garmentServiceSubconShrinkagePanelRepository.Update(subconPanel);
                }
                else if (subconDeliveryLetterOut.SubconCategory == "SUBCON BB FABRIC WASH/PRINT")
                {
                    var subconFabric = _garmentServiceSubconFabricWashRepository.Query.Where(x => x.Identity == subconDeliveryLetterOutItem.SubconId).Select(s => new GarmentServiceSubconFabricWash(s)).Single();
                    subconFabric.SetIsUsed(false);
                    subconFabric.Modify();

                    await _garmentServiceSubconFabricWashRepository.Update(subconFabric);
                }
                await _garmentSubconDeliveryLetterOutItemRepository.Update(subconDeliveryLetterOutItem);
            });

            //var subconDLOuts = _garmentSubconDeliveryLetterOutRepository.Query.Where(o => o.SubconContractId == subconDeliveryLetterOut.SubconContractId && o.Identity!=request.Identity).FirstOrDefault();
            //if (subconDLOuts == null)
            //{
            //    var subconContract = _garmentSubconContractRepository.Query.Where(x => x.Identity == subconDeliveryLetterOut.SubconContractId).Select(s => new GarmentSubconContract(s)).Single();
            //    subconContract.SetIsUsed(false);
            //    subconContract.Modify();

            //    await _garmentSubconContractRepository.Update(subconContract);
            //}

            subconDeliveryLetterOut.Remove();
            await _garmentSubconDeliveryLetterOutRepository.Update(subconDeliveryLetterOut);

            _storage.Save();

            return subconDeliveryLetterOut;
        }
    }
}
