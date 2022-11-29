using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class UpdateGarmentSewingOutCommandHandler : ICommandHandler<UpdateGarmentSewingOutCommand, GarmentSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public UpdateGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSewingOut> Handle(UpdateGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSewingOut(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            if (sewOut.SewingTo == "CUTTING")
            {
                Guid cutInId = _garmentCuttingInItemRepository.Query.Where(a => a.SewingOutId == sewOut.Identity).First().CutInId;

                var cutIn = _garmentCuttingInRepository.Query.Where(a => a.Identity == cutInId).Select(a => new GarmentCuttingIn(a)).Single();

                _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
                {
                    cutInItem.Modify();
                    await _garmentCuttingInItemRepository.Update(cutInItem);
                });
                cutIn.SetDate(request.SewingOutDate.GetValueOrDefault());
                cutIn.Modify();
                await _garmentCuttingInRepository.Update(cutIn);
            }

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                var item = request.Items.Where(o => o.Id == sewOutItem.Identity).Single();

                var diffSewInQuantity = item.IsSave ? (sewOutItem.Quantity - (request.IsDifferentSize ? item.TotalQuantity : item.Quantity ) ): sewOutItem.Quantity;

                if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SewingInItemId))
                {
                    sewInItemToBeUpdated[sewOutItem.SewingInItemId] += diffSewInQuantity;
                }
                else
                {
                    sewInItemToBeUpdated.Add(sewOutItem.SewingInItemId, diffSewInQuantity);
                }

                if (!item.IsSave)
                {
                    item.Quantity = 0;
                    
                    if (request.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            if (sewOut.SewingTo == "CUTTING")
                            {
                                GarmentCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentCuttingInDetail(a)).Single();
                                cuttingInDetail.Remove();
                                await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                            }
                            
                            sewOutDetail.Remove();
                            await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                        });

                        if (sewOut.SewingTo == "CUTTING")
                        {
                            Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == item.Details.First().Id).Select(a => new GarmentCuttingInDetail(a)).First().CutInItemId;
                            GarmentCuttingInItem cuttingInItem = _garmentCuttingInItemRepository.Query.Where(a => a.Identity == cuttingInItemId).Select(a => new GarmentCuttingInItem(a)).Single();
                            cuttingInItem.Remove();
                            await _garmentCuttingInItemRepository.Update(cuttingInItem);
                        }

                    }
                    else
                    {
                        if (sewOut.SewingTo == "CUTTING")
                        {
                            GarmentCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentCuttingInDetail(a)).Single();
                            GarmentCuttingInItem cuttingInItem = _garmentCuttingInItemRepository.Query.Where(a => a.Identity == cuttingInDetail.CutInItemId).Select(a => new GarmentCuttingInItem(a)).Single();
                            cuttingInDetail.Remove();
                            await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                            cuttingInItem.Remove();
                            await _garmentCuttingInItemRepository.Update(cuttingInItem);
                        }
                    }

                    sewOutItem.Remove();

                }
                else
                {
                    if (request.IsDifferentSize)
                    {
                        
                        _garmentSewingOutDetailRepository.Find(o => o.SewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            if (sewOutDetail.Identity != Guid.Empty)
                            {
                                
                                var detail = item.Details.Where(o => o.Id == sewOutDetail.Identity).SingleOrDefault();

                                if (detail != null)
                                {
                                    if (sewOut.SewingTo == "CUTTING")
                                    {
                                        GarmentCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentCuttingInDetail(a)).Single();
                                        cuttingInDetail.SetCuttingInQuantity(Convert.ToInt32(detail.Quantity));
                                        cuttingInDetail.SetPrice((item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.Quantity);
                                        cuttingInDetail.SetRemainingQuantity(Convert.ToInt32(detail.Quantity));
                                        cuttingInDetail.Modify();
                                        await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                                    }
                                    sewOutDetail.SetQuantity(detail.Quantity);
                                    sewOutDetail.SetSizeId(new SizeId(detail.Size.Id));
                                    sewOutDetail.SetSizeName(detail.Size.Size);

                                    sewOutDetail.Modify();
                                }
                                else
                                {
                                    if (sewOut.SewingTo == "CUTTING")
                                    {
                                        GarmentCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutDetailId == sewOutDetail.Identity).Select(a => new GarmentCuttingInDetail(a)).Single();
                                        cuttingInDetail.Remove();
                                        await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                                    }
                                    sewOutDetail.Remove();
                                }
                                await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                            }
                            else
                            {
                                GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                    Guid.NewGuid(),
                                    sewOutItem.Identity,
                                    sewOutDetail.SizeId,
                                    sewOutDetail.SizeName,
                                    sewOutDetail.Quantity,
                                    sewOutDetail.UomId,
                                    sewOutDetail.UomUnit
                                    );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                                if (sewOut.SewingTo == "CUTTING")
                                {
                                    Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentCuttingInDetail(a)).First().CutInItemId;
                                    GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    cuttingInItemId,
                                    Guid.Empty,
                                    sewOutItem.Identity,
                                    garmentSewingOutDetail.Identity,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(garmentSewingOutDetail.Quantity),
                                    garmentSewingOutDetail.UomId,
                                    garmentSewingOutDetail.UomUnit,
                                    garmentSewingOutDetail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * garmentSewingOutDetail.Quantity,
                                    0,
                                    item.Color
                                    );

                                    await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                                }
                            }
                            
                        });

                        foreach(var detail in item.Details)
                        {
                            if (detail.Id == Guid.Empty)
                            {
                                GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                    Guid.NewGuid(),
                                    sewOutItem.Identity,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit
                                );
                                await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                                if (sewOut.SewingTo == "CUTTING")
                                {
                                    Guid cuttingInItemId = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentCuttingInDetail(a)).First().CutInItemId;
                                    GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    cuttingInItemId,
                                    Guid.Empty,
                                    sewOutItem.Identity,
                                    garmentSewingOutDetail.Identity,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(garmentSewingOutDetail.Quantity),
                                    garmentSewingOutDetail.UomId,
                                    garmentSewingOutDetail.UomUnit,
                                    garmentSewingOutDetail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * garmentSewingOutDetail.Quantity,
                                    0,
                                    item.Color
                                    );

                                    await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                                }
                            }
                        }
                        sewOutItem.SetQuantity(item.TotalQuantity);
                        sewOutItem.SetRemainingQuantity(item.TotalQuantity);
                    }
                    else
                    {
                        sewOutItem.SetQuantity(item.Quantity);
                        sewOutItem.SetRemainingQuantity(item.Quantity);
                        if (sewOut.SewingTo == "CUTTING")
                        {
                            GarmentCuttingInDetail cuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(a => a.SewingOutItemId == sewOutItem.Identity).Select(a => new GarmentCuttingInDetail(a)).Single();
                            cuttingInDetail.SetCuttingInQuantity(Convert.ToInt32(sewOutItem.Quantity));
                            cuttingInDetail.SetPrice((item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * sewOutItem.Quantity);
                            cuttingInDetail.SetRemainingQuantity(Convert.ToInt32(sewOutItem.Quantity));
                            
                            cuttingInDetail.Modify();
                            await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                        }

                    }

                    sewOutItem.SetPrice(item.Price);
                    sewOutItem.Modify();
                }

                
                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.SetDate(request.SewingOutDate.GetValueOrDefault());
            sewOut.Modify();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
