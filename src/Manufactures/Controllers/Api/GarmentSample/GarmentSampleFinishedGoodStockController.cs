using Barebone.Controllers;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Dtos.GarmentSample.SampleFinishedGoodStocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-finished-good-stocks")]
    public class GarmentSampleFinishedGoodStockController : ControllerApiBase
    {
        private readonly IGarmentSampleFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        public GarmentSampleFinishedGoodStockController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentFinishedGoodStockRepository = Storage.GetRepository<IGarmentSampleFinishedGoodStockRepository>();

        }
        //[HttpGet]
        //public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        //{
        //    VerifyUser();

        //    var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
        //    var count = query.Count();

        //    List<GarmentSampleFinishedGoodStockAdjustmentDto> listDtos = _garmentFinishedGoodStockRepository
        //                    .Find(query)
        //                    .Where(data => data.Quantity > 0)
        //                    .Select(data => new GarmentSampleFinishedGoodStockAdjustmentDto(data))
        //                    .ToList();

        //    await Task.Yield();
        //    return Ok(listDtos, info: new
        //    {
        //        page,
        //        size,
        //        count
        //    });
        //}

        [HttpGet("list")]
        public async Task<IActionResult> GetList(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            List<GarmentSampleFinishedGoodStockDto> listDtos = _garmentFinishedGoodStockRepository
                            .Find(query)
                            .Where(data => data.Quantity > 0)
                            .Select(data => new GarmentSampleFinishedGoodStockDto(data))
                            .ToList();

            await Task.Yield();
            return Ok(listDtos, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentFinishedGoodStockRepository.ReadComplete(page, size, order, keyword, filter);
            var count = query.Count();

            List<GarmentSampleFinishedGoodStockDto> listDtos = _garmentFinishedGoodStockRepository
                            .Find(query)
                            .Where(data => data.Quantity > 0)
                            .Select(data => new GarmentSampleFinishedGoodStockDto(data))
                            .ToList();

            await Task.Yield();
            return Ok(listDtos, info: new
            {
                page,
                size,
                count
            });
        }

        //[HttpGet("complete")]
        //public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        //{
        //    VerifyUser();
        //    var query = _garmentFinishedGoodStockRepository.Read(page, size, order, keyword, filter);
        //    var count = query.Count();
        //    var garmentFinishedGoodStockDto = _garmentFinishedGoodStockRepository.Find(query).Select(o => new GarmentSampleFinishedGoodStockAdjustmentDto(o)).Where(o => o.Quantity > 0).ToArray();
        //    if (order != "{}")
        //    {
        //        Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
        //        garmentFinishedGoodStockDto = QueryHelper<GarmentSampleFinishedGoodStockAdjustmentDto>.Order(garmentFinishedGoodStockDto.AsQueryable(), OrderDictionary).ToArray();
        //    }
        //    await Task.Yield();
        //    return Ok(garmentFinishedGoodStockDto, info: new
        //    {
        //        page,
        //        size,
        //        count
        //    });
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(string id)
        //{
        //    Guid guid = Guid.Parse(id);

        //    VerifyUser();

        //    GarmentSampleFinishedGoodStockAdjustmentDto garmentFinishingInDto = _garmentFinishedGoodStockRepository.Find(o => o.Identity == guid).Select(loading => new GarmentSampleFinishedGoodStockAdjustmentDto(loading)).FirstOrDefault();

        //    await Task.Yield();
        //    return Ok(garmentFinishingInDto);
        //}

    }
}
