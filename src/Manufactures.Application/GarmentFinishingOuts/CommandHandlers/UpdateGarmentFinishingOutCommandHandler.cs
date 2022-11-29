using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingOuts.CommandHandlers
{
    public class UpdateGarmentFinishingOutCommandHandler : ICommandHandler<UpdateGarmentFinishingOutCommand, GarmentFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public UpdateGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(UpdateGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _garmentFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finishOutItem =>
            {
                var item = request.Items.Where(o => o.Id == finishOutItem.Identity).Single();

                var diffSewInQuantity = item.IsSave ? (finishOutItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity)) : finishOutItem.Quantity;

                if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                {
                    finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += diffSewInQuantity;
                }
                else
                {
                    finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, diffSewInQuantity);
                }

                if (!item.IsSave)
                {
                    item.Quantity = 0;

                    if (request.IsDifferentSize)
                    {
                        _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                        {
                            finishOutDetail.Remove();
                            await _garmentFinishingOutDetailRepository.Update(finishOutDetail);
                        });
                    }

                    finishOutItem.Remove();

                }
                else
                {


                    if (request.IsDifferentSize)
                    {
                        _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                        {
                            if (finishOutDetail.Identity != Guid.Empty)
                            {
                                var detail = item.Details.Where(o => o.Id == finishOutDetail.Identity).SingleOrDefault();

                                if (detail != null)
                                {
                                    finishOutDetail.SetQuantity(detail.Quantity);
                                    finishOutDetail.SetSizeId(new SizeId(detail.Size.Id));
                                    finishOutDetail.SetSizeName(detail.Size.Size);

                                    finishOutDetail.Modify();

                                }
                                else
                                {
                                    finishOutDetail.Remove();
                                }
                                await _garmentFinishingOutDetailRepository.Update(finishOutDetail);
                            }
                            else
                            {
                                GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(
                                Guid.NewGuid(),
                                finishOutItem.Identity,
                                finishOutDetail.SizeId,
                                finishOutDetail.SizeName,
                                finishOutDetail.Quantity,
                                finishOutDetail.UomId,
                                finishOutDetail.UomUnit
                            );
                                await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);
                            }

                        });

                        foreach (var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty)
                            {
                                GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(
                                    Guid.NewGuid(),
                                    finishOutItem.Identity,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit
                                );
                                await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);
                            }
                        }
                        finishOutItem.SetQuantity(item.TotalQuantity);
                        finishOutItem.SetRemainingQuantity(item.TotalQuantity);
                    }
                    else
                    {
                        finishOutItem.SetQuantity(item.Quantity);
                        finishOutItem.SetRemainingQuantity(item.Quantity);
                    }

                    finishOutItem.SetPrice(item.Price);
                    finishOutItem.Modify();
                }


                await _garmentFinishingOutItemRepository.Update(finishOutItem);
            });

            foreach (var finishingInItem in finishingInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finishingInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + finishingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentFinishingInItemRepository.Update(garmentSewInItem);
            }

            finishOut.SetDate(request.FinishingOutDate.GetValueOrDefault());
            finishOut.Modify();
            await _garmentFinishingOutRepository.Update(finishOut);

            _storage.Save();

            return finishOut;
        }
    }
}
