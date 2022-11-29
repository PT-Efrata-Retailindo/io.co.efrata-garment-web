using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSubcon.CustomsOuts.CommandHandlers
{
    public class UpdateGarmentSubconCustomsOutCommandHandler : ICommandHandler<UpdateGarmentSubconCustomsOutCommand, GarmentSubconCustomsOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSubconCustomsOutRepository _garmentSubconCustomsOutRepository;
        private readonly IGarmentSubconCustomsOutItemRepository _garmentSubconCustomsOutItemRepository;
        private readonly IGarmentSubconDeliveryLetterOutRepository _garmentSubconDeliveryLetterOutRepository;

        public UpdateGarmentSubconCustomsOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSubconCustomsOutRepository = storage.GetRepository<IGarmentSubconCustomsOutRepository>();
            _garmentSubconCustomsOutItemRepository = storage.GetRepository<IGarmentSubconCustomsOutItemRepository>();
            _garmentSubconDeliveryLetterOutRepository = storage.GetRepository<IGarmentSubconDeliveryLetterOutRepository>();
        }

        public async Task<GarmentSubconCustomsOut> Handle(UpdateGarmentSubconCustomsOutCommand request, CancellationToken cancellationToken)
        {
            var subconCustomsOut = _garmentSubconCustomsOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSubconCustomsOut(o)).Single();

            
            _garmentSubconCustomsOutItemRepository.Find(o => o.SubconCustomsOutId == subconCustomsOut.Identity).ForEach(async subconDLItem =>
            {
                var item = request.Items.Where(o => o.Id == subconDLItem.Identity).SingleOrDefault();

                if (item == null)
                {
                    var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == subconDLItem.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                    subconDLOut.SetIsUsed(false);
                    subconDLOut.Modify();
                    await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);
                    subconDLItem.Remove();
                }
                else
                {
                    subconDLItem.Modify();
                }


                await _garmentSubconCustomsOutItemRepository.Update(subconDLItem);
            });

            foreach (var item in request.Items)
            {
                if (item.Id == Guid.Empty)
                {
                    GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(
                        Guid.NewGuid(),
                        subconCustomsOut.Identity,
                        item.SubconDLOutNo,
                        item.SubconDLOutId,
                        item.Quantity
                    );
                    var subconDLOut = _garmentSubconDeliveryLetterOutRepository.Query.Where(x => x.Identity == item.SubconDLOutId).Select(s => new GarmentSubconDeliveryLetterOut(s)).Single();
                    subconDLOut.SetIsUsed(true);
                    subconDLOut.Modify();
                    await _garmentSubconDeliveryLetterOutRepository.Update(subconDLOut);

                    await _garmentSubconCustomsOutItemRepository.Update(garmentSubconCustomsOutItem);
                }
            }
            

            subconCustomsOut.SetDate(request.CustomsOutDate);
            subconCustomsOut.SetRemark(request.Remark);
            subconCustomsOut.SetSubconCategory(request.SubconCategory);
            subconCustomsOut.SetCustomsOutNo(request.CustomsOutNo);
            subconCustomsOut.SetCustomsOutType(request.CustomsOutType);

            subconCustomsOut.Modify();

            await _garmentSubconCustomsOutRepository.Update(subconCustomsOut);

            _storage.Save();

            return subconCustomsOut;
        }
    }
}
