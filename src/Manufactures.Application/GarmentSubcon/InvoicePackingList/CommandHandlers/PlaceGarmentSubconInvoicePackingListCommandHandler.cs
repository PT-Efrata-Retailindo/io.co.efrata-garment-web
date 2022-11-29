using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Manufactures.Application.GarmentSubcon.InvoicePackingList.CommandHandlers
{
    public class PlaceGarmentSubconInvoicePackingListCommandHandler : ICommandHandler<PlaceGarmentSubconInvoicePackingListCommand, SubconInvoicePackingList>
    {
        private readonly IStorage _storage;
        private readonly ISubconInvoicePackingListRepository _subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository _subconInvoicePackingListItemRepository;

        public PlaceGarmentSubconInvoicePackingListCommandHandler(IStorage storage)
        {
            _storage = storage;
            _subconInvoicePackingListRepository = storage.GetRepository<ISubconInvoicePackingListRepository>();
            _subconInvoicePackingListItemRepository = storage.GetRepository<ISubconInvoicePackingListItemRepository>();
        }
        public async Task<SubconInvoicePackingList> Handle(PlaceGarmentSubconInvoicePackingListCommand request, CancellationToken cancellationToken)
        {
            Guid InvoicePackingListId = Guid.NewGuid();
            SubconInvoicePackingList subconInvoicePackingList = new SubconInvoicePackingList(
                InvoicePackingListId,
                GenerateNo(request),
                request.BCType,
                request.Date,
                new SupplierId(request.Supplier.Id),
                request.Supplier.Code,
                request.Supplier.Name,
                request.Supplier.Address,
                request.ContractNo,
                request.NW,
                request.GW,
                request.Remark
                );

            foreach(var item in request.Items)
            {
                SubconInvoicePackingListItem subconInvoicePackingListItem = new SubconInvoicePackingListItem(
                    Guid.NewGuid(),
                    InvoicePackingListId,
                    item.DLNo,
                    item.DLDate,
                    //new ProductId(item.Product.Id),
                    //item.Product.Code,
                    //item.Product.Name,
                    //item.Product.Remark,
                    // item.DesignColor,

                    new ProductId(0),
                    "-",
                    "-",
                    "-",
                    "-",
                    item.Quantity,
                    //new UomId(item.Uom.Id),
                    //item.Uom.Unit,
                    new UomId(0),
                    "-",
                    item.CIF,
                    item.TotalPrice
                    );
                await _subconInvoicePackingListItemRepository.Update(subconInvoicePackingListItem);
            }
            await _subconInvoicePackingListRepository.Update(subconInvoicePackingList);
            _storage.Save();

            return subconInvoicePackingList;
        }
        private string GenerateNo(PlaceGarmentSubconInvoicePackingListCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.BCType == "BC 2.6.1" ? "M" : "K";
            //var type = request.ContractType == "SUBCON BAHAN BAKU" ? "BB" : request.ContractType == "SUBCON CUTTING" ? "CT" : request.ContractType == "SUBCON GARMENT" ? "SG" : "JS";

            var prefix = $"IV{code}{year}{month}";

            var lastNo = _subconInvoicePackingListRepository.Query.Where(w => w.InvoiceNo.StartsWith(prefix))
                .OrderByDescending(o => o.InvoiceNo)
                .Select(s => int.Parse(s.InvoiceNo.Substring(11, 2)))
                .FirstOrDefault();
            var no = $"{prefix}{(lastNo + 1).ToString("D5")}";

            return no;
        }
    }
}
