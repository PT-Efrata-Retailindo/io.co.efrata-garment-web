using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingIns.Queries
{
    public class GetAllSampleSewingInQueryHandler : IQueryHandler<GetAllSampleSewingInQuery, SampleSewingInListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;

        public GetAllSampleSewingInQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }

        public async Task<SampleSewingInListViewModel> Handle(GetAllSampleSewingInQuery request, CancellationToken cancellationToken)
        {
            var SewingInQuery = _garmentSewingInRepository.Query;
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.filter);
            SewingInQuery = QueryHelper<GarmentSampleSewingInReadModel>.Filter(SewingInQuery, FilterDictionary);
            int total = SewingInQuery.Count();


            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(request.order);
            SewingInQuery = OrderDictionary.Count == 0 ? SewingInQuery.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingInReadModel>.Order(SewingInQuery, OrderDictionary);

            if (!string.IsNullOrWhiteSpace(request.keyword))
            {
                SewingInQuery = SewingInQuery
                    .Where(co => co.SewingInNo.Contains(request.keyword)
                    || co.UnitCode.Contains(request.keyword)
                    || co.RONo.Contains(request.keyword)
                    || co.Article.Contains(request.keyword)
                    || co.GarmentSampleSewingInItem.Any(coi => coi.SewingInId == co.Identity && coi.ProductCode.Contains(request.keyword)));
            }

            var DocId = SewingInQuery.Select(x => x.Identity);
            var queryItem = _garmentSewingInItemRepository.Query.Where(x => DocId.Contains(x.SewingInId));
            //var queryDetail = _garmentSewingInItemRepository.Query.Where(x => DocItemId.Contains(x.SewingInItemId));
            double totalQty = queryItem.Sum(x => x.Quantity);

            var selectedQuery = SewingInQuery.Select(co => new GarmentSampleSewingInListDto
            {
                Id = co.Identity,
                SewingInNo = co.SewingInNo,
                SewingFrom = co.SewingFrom,
                UnitFrom = new UnitDepartment(co.UnitFromId, co.UnitFromCode, co.UnitFromName),
                SewingInDate = co.SewingInDate,
                RONo = co.RONo,
                Article = co.Article,
                Unit = new UnitDepartment(co.UnitId, co.UnitCode, co.UnitName),
                Comodity = new GarmentComodity(co.ComodityId, co.ComodityCode, co.ComodityName),
                

            });

            //var selectedData = selectedQuery.ToList();
            var selectedData = selectedQuery
                .Skip((request.page - 1) * request.size)
                .Take(request.size)
                .ToList();

            foreach (var co in selectedData)
            {
                co.Items = _garmentSewingInItemRepository.Query.Where(x => x.SewingInId == co.Id).OrderBy(x => x.Identity).Select(coi => new GarmentSampleSewingInItemDto
                {
                    Id = coi.Identity,
                    CuttingOutItemId = coi.CuttingOutItemId,
                    CuttingOutDetailId = coi.CuttingOutDetailId,
                    Color = coi.Color,
                    Product = new Product(coi.ProductId, coi.ProductCode, coi.ProductName),
                    DesignColor = coi.DesignColor,
                    Quantity = coi.Quantity,
                    RemainingQuantity=coi.RemainingQuantity,
                    Size = new SizeValueObject(coi.SizeId,coi.SizeName),
                    Uom = new Uom(coi.UomId,coi.UomUnit)
                }).ToList();


                co.Products = co.Items.Select(i => i.Product.Code).ToList();
                co.TotalQuantity = co.Items.Sum(d => d.Quantity);
                co.TotalRemainingQuantity = co.Items.Sum(d => d.RemainingQuantity);
            }

            await Task.Yield();
            return new SampleSewingInListViewModel
            {
                data = selectedData,
                total = total,
                totalQty = totalQty
            };
        }
    }
}
