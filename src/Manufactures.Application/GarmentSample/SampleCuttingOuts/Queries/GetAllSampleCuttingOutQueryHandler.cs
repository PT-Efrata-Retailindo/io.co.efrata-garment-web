using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries
{
    public class GetAllSampleCuttingOutQueryHandler : IQueryHandler<GetAllSampleCuttingOutQuery, SampleCuttingOutListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetAllSampleCuttingOutQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
        }

        public async Task<SampleCuttingOutListViewModel> Handle(GetAllSampleCuttingOutQuery request, CancellationToken cancellationToken)
        {
            var cuttingOutQuery = _garmentCuttingOutRepository.Query.Where(co => co.CuttingOutType != "SUBKON");
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.filter);
            cuttingOutQuery = QueryHelper<GarmentSampleCuttingOutReadModel>.Filter(cuttingOutQuery, FilterDictionary);
            int total = cuttingOutQuery.Count();


            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.order);
            cuttingOutQuery = OrderDictionary.Count == 0 ? cuttingOutQuery.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleCuttingOutReadModel>.Order(cuttingOutQuery, OrderDictionary);

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                cuttingOutQuery = cuttingOutQuery
                    .Where(co => co.CutOutNo.Contains(request.keyword)
                    || co.UnitCode.Contains(request.keyword)
                    || co.RONo.Contains(request.keyword)
                    || co.Article.Contains(request.keyword)
                    || co.GarmentSampleCuttingOutItem.Any(coi => coi.CuttingOutId == co.Identity && coi.ProductCode.Contains(request.keyword)));
            }

            var DocId = cuttingOutQuery.Select(x => x.Identity);
            var DocItemId = _garmentCuttingOutItemRepository.Query.Where(x => DocId.Contains(x.CuttingOutId)).Select(x => x.Identity);
            var queryDetail = _garmentCuttingOutDetailRepository.Query.Where(x => DocItemId.Contains(x.CuttingOutItemId));
            double totalQty = queryDetail.Sum(x => x.CuttingOutQuantity);

            var selectedQuery = cuttingOutQuery.Select(co => new GarmentSampleCuttingOutListDto
            {
                Id = co.Identity,
                CutOutNo = co.CutOutNo,
                CuttingOutType = co.CuttingOutType,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                CuttingOutDate = co.CuttingOutDate,
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName)
            });

            //var selectedData = selectedQuery.ToList();
            var selectedData = selectedQuery
                .Skip((request.page - 1) * request.size)
                .Take(request.size)
                .ToList();

            foreach (var co in selectedData)
            {
                co.Items = _garmentCuttingOutItemRepository.Query.Where(x => x.CuttingOutId == co.Id).OrderBy(x => x.Identity).Select(coi => new GarmentSampleCuttingOutItemDto
                {
                    Id = coi.Identity,
                    CutOutId = coi.CuttingOutId,
                    CuttingInId = coi.CuttingInId,
                    CuttingInDetailId = coi.CuttingInDetailId,
                    Product = new Product(coi.ProductId, coi.ProductCode, coi.ProductName),
                    DesignColor = coi.DesignColor,
                    TotalCuttingOut = coi.TotalCuttingOut,
                }).ToList();

                foreach (var coi in co.Items)
                {
                    coi.Details = _garmentCuttingOutDetailRepository.Query.Where(x => x.CuttingOutItemId == coi.Id).OrderBy(x => x.Identity).Select(cod => new GarmentSampleCuttingOutDetailDto
                    {
                        Id = cod.Identity,
                        CutOutItemId = cod.CuttingOutItemId,
                        Size = new SizeValueObject(cod.SizeId, cod.SizeName),
                        CuttingOutQuantity = cod.CuttingOutQuantity,
                        CuttingOutUom = new Uom(cod.CuttingOutUomId, cod.CuttingOutUomUnit),
                        Color = cod.Color,
                        RemainingQuantity = cod.RemainingQuantity,
                        BasicPrice = cod.BasicPrice,
                        Price = cod.Price,
                    }).ToList();
                }

                co.Products = co.Items.Select(i => i.Product.Code).ToList();
                co.TotalCuttingOutQuantity = co.Items.Sum(i => i.Details.Sum(d => d.CuttingOutQuantity));
                co.TotalRemainingQuantity = co.Items.Sum(i => i.Details.Sum(d => d.RemainingQuantity));
            }

            await Task.Yield();
            return new SampleCuttingOutListViewModel
            {
                data = selectedData,
                total = total,
                totalQty = totalQty
            };
        }
    }
}
