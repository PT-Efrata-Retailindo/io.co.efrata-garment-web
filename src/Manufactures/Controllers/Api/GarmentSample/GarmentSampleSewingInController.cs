using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Dtos.GarmentSample.SampleSewingIns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-sewing-ins")]
    public class GarmentSampleSewingInController : ControllerApiBase
    {
        private readonly IGarmentSampleSewingInRepository _GarmentSampleSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _GarmentSampleSewingInItemRepository;

        public GarmentSampleSewingInController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _GarmentSampleSewingInRepository = Storage.GetRepository<IGarmentSampleSewingInRepository>();
            _GarmentSampleSewingInItemRepository = Storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var cuttingOutQuery = new Application.GarmentSample.SampleSewingIns.Queries.GetAllSampleSewingInQuery(page, size, order, keyword, filter);
            var viewModel = await Mediator.Send(cuttingOutQuery);
            return Ok(viewModel.data, info: new
            {
                page,
                size,
                viewModel.total,
                count = viewModel.data.Count,
                viewModel.totalQty
            });
        }

        [HttpGet("get-by-ro")]
        public async Task<IActionResult> GetByRo(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _GarmentSampleSewingInRepository.ReadComplete(page, size, order, keyword, filter);
            var total = query.Count();
            double totalQty = query.Sum(a => a.GarmentSampleSewingInItem.Sum(b => b.Quantity));
            query = query.Skip((page - 1) * size).Take(size);

            var GarmentSampleSewingInDto = _GarmentSampleSewingInRepository.Find(query).Select(o => new GarmentSampleSewingInListDto(o)).ToArray();

            await Task.Yield();
            return Ok(GarmentSampleSewingInDto, info: new
            {
                page,
                size,
                total,
                totalQty
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSampleSewingInDto GarmentSampleSewingIn = _GarmentSampleSewingInRepository.Find(o => o.Identity == guid).Select(sewingIn => new GarmentSampleSewingInDto(sewingIn)
            {
                Items = _GarmentSampleSewingInItemRepository.Find(o => o.SewingInId == sewingIn.Identity).OrderBy(i => i.Color).ThenBy(i => i.SizeName).Select(sewingInItem => new GarmentSampleSewingInItemDto(sewingInItem)).ToList()

            }
            ).FirstOrDefault();

            await Task.Yield();
            return Ok(GarmentSampleSewingIn);
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _GarmentSampleSewingInRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var GarmentSampleSewingInDto = _GarmentSampleSewingInRepository.Find(query).Select(o => new GarmentSampleSewingInDto(o)).ToArray();
            var GarmentSampleSewingInItemDto = _GarmentSampleSewingInItemRepository.Find(_GarmentSampleSewingInItemRepository.Query).Select(o => new GarmentSampleSewingInItemDto(o)).ToList();

            Parallel.ForEach(GarmentSampleSewingInDto, itemDto =>
            {
                var GarmentSampleSewingInItems = GarmentSampleSewingInItemDto.Where(x => x.SewingInId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = GarmentSampleSewingInItems;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                GarmentSampleSewingInDto = QueryHelper<GarmentSampleSewingInDto>.Order(GarmentSampleSewingInDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(GarmentSampleSewingInDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("update-dates")]
        public async Task<IActionResult> UpdateDates([FromBody]UpdateDatesGarmentSampleSewingInCommand command)
        {
            VerifyUser();

            if (command.Date == null || command.Date == DateTimeOffset.MinValue)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal harus diisi"
                });
            else if (command.Date.Date > DateTimeOffset.Now.Date)
                return BadRequest(new
                {
                    code = HttpStatusCode.BadRequest,
                    error = "Tanggal tidak boleh lebih dari hari ini"
                });

            var order = await Mediator.Send(command);

            return Ok();
        }
    }
}
