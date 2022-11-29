using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers
{
    public class UpdateGarmentSubconContractCommandHandler : ICommandHandler<UpdateGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository _garmentSubconContractItemRepository;

        public UpdateGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentSubconContractItemRepository = storage.GetRepository<IGarmentSubconContractItemRepository>();
        }

        public async Task<GarmentSubconContract> Handle(UpdateGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            var subconContract = _garmentSubconContractRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconContract(o)).Single();
            _garmentSubconContractItemRepository.Find(o => o.SubconContractId == subconContract.Identity).ForEach(async subconItem =>
            {
                var item = request.Items.Where(o => o.Id == subconItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    subconItem.Remove();
                }
                else
                {
                    subconItem.SetCIFItem(item.CIFItem);
                    subconItem.SetQuantity(item.Quantity);
                    subconItem.Modify();
                }
                await _garmentSubconContractItemRepository.Update(subconItem);
            });
            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconContractItem garmentSubconContractItem = new GarmentSubconContractItem(
                        Guid.NewGuid(),
                        subconContract.Identity,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.CIFItem
                    );
                    
                    await _garmentSubconContractItemRepository.Update(garmentSubconContractItem);
                }
            }
            subconContract.SetAgreementNo(request.AgreementNo);
            subconContract.SetBPJNo(request.BPJNo);
            subconContract.SetContractNo(request.ContractNo);
            subconContract.SetContractType(request.ContractType);
            subconContract.SetDueDate(request.DueDate);
            subconContract.SetFinishedGoodType(request.FinishedGoodType);
            subconContract.SetJobType(request.JobType);
            subconContract.SetQuantity(request.Quantity);
            subconContract.SetSupplierCode(request.Supplier.Code);
            subconContract.SetSupplierId(new SupplierId(request.Supplier.Id));
            subconContract.SetSupplierName(request.Supplier.Name);
            subconContract.SetContractDate(request.ContractDate);
            subconContract.SetIsUsed(request.IsUsed);
            subconContract.SetBuyerId(new BuyerId(request.Buyer.Id));
            subconContract.SetBuyerCode(request.Buyer.Code);
            subconContract.SetBuyerName(request.Buyer.Name);
            subconContract.SetSKEPNo(request.SKEPNo);
            subconContract.SetSubconCategory(request.SubconCategory);
            subconContract.SetUomId(new UomId(request.Uom.Id));
            subconContract.SetUomUnit(request.Uom.Unit);
            subconContract.SetAgreementDate(request.AgreementDate);
            subconContract.SetCIF(request.CIF);

            subconContract.Modify();
            await _garmentSubconContractRepository.Update(subconContract);

            _storage.Save();

            return subconContract;
        }
    }
}
