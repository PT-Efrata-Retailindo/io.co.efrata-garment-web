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
    public class PlaceGarmentSubconContractCommandHandler : ICommandHandler<PlaceGarmentSubconContractCommand, GarmentSubconContract>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository _garmentSubconContractItemRepository;

        public PlaceGarmentSubconContractCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconContractRepository = storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentSubconContractItemRepository= storage.GetRepository<IGarmentSubconContractItemRepository>();
        }

        public async Task<GarmentSubconContract> Handle(PlaceGarmentSubconContractCommand request, CancellationToken cancellationToken)
        {
            Guid SubconContractId = Guid.NewGuid();

            GarmentSubconContract garmentSubconContract = new GarmentSubconContract(
                SubconContractId,
                request.ContractType,
                GenerateNo(request),
                request.AgreementNo,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.JobType,
                request.BPJNo,
                request.FinishedGoodType,
                request.Quantity,
                request.DueDate,
                request.ContractDate,
                request.IsUsed,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                request.SubconCategory,
                new UomId(request.Uom.Id),
                request.Uom.Unit,
                request.SKEPNo,
                request.AgreementDate,
                request.CIF
            );

            foreach (var item in request.Items)
            {
                GarmentSubconContractItem garmentSubconContractItem = new GarmentSubconContractItem(
                    Guid.NewGuid(),
                    garmentSubconContract.Identity,
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

            await _garmentSubconContractRepository.Update(garmentSubconContract);
            _storage.Save();

            return garmentSubconContract;
        }

        private string GenerateNo(PlaceGarmentSubconContractCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yyyy");
            var month = now.ToString("MM");

            var prefix = $"{year}";

            var lastContractNo = _garmentSubconContractRepository.Query.Where(w => w.ContractNo.EndsWith(prefix))
                .OrderByDescending(o => o.ContractNo)
                .Select(s => int.Parse(s.ContractNo.Substring(0,3)))
                .FirstOrDefault();
            var ContractNo = $"{(lastContractNo + 1).ToString("D3")}/AG/{request.Supplier.Code}/{month}/{prefix}";

            return ContractNo;
        }
    }
}
