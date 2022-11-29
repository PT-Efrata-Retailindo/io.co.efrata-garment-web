using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers
{
    public class UpdateGarmentSubconCustomsInCommandHandler : ICommandHandler<UpdateGarmentSubconCustomsInCommand, GarmentSubconCustomsIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsInRepository _garmentSubconCustomsInRepository;
        private readonly IGarmentSubconCustomsInItemRepository _garmentSubconCustomsInItemRepository;

        public UpdateGarmentSubconCustomsInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsInRepository = storage.GetRepository<IGarmentSubconCustomsInRepository>();
            _garmentSubconCustomsInItemRepository = storage.GetRepository<IGarmentSubconCustomsInItemRepository>();
        }

        public async Task<GarmentSubconCustomsIn> Handle(UpdateGarmentSubconCustomsInCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsIn = _garmentSubconCustomsInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsIn(o)).Single();

            _garmentSubconCustomsInItemRepository.Find(o => o.SubconCustomsInId == subconCustomsIn.Identity).ForEach(async subconCustomsItem =>
            {
                var item = request.Items.Where(o => o.Id == subconCustomsItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    subconCustomsItem.Remove();
                }
                else
                {
                    subconCustomsItem.Modify();
                }


                await _garmentSubconCustomsInItemRepository.Update(subconCustomsItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconCustomsInItem garmentSubconCustomsInItem = new GarmentSubconCustomsInItem(
                        Guid.NewGuid(),
                        subconCustomsIn.Identity,
                        new SupplierId(item.Supplier.Id),
                        item.Supplier.Code,
                        item.Supplier.Name,
                        item.DoId,
                        item.DoNo,
                        item.Quantity
                    );

                    await _garmentSubconCustomsInItemRepository.Update(garmentSubconCustomsInItem);
                }
            }

            subconCustomsIn.SetBcDate(request.BcDate.GetValueOrDefault());
            subconCustomsIn.SetBcNo(request.BcNo);
            subconCustomsIn.SetBcType(request.BcType);
            subconCustomsIn.SetRemark(request.Remark);
            subconCustomsIn.Modify();
            await _garmentSubconCustomsInRepository.Update(subconCustomsIn);

            _storage.Save();

            return subconCustomsIn;
        }
    }
}
