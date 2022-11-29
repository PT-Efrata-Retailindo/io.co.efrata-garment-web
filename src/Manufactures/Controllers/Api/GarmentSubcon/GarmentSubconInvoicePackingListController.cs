using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
//using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.ExcelTemplates;
//using Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport;
//using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Commands;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.ReadModels;
using Manufactures.Domain.GarmentSubcon.InvoicePackingList.Repositories;
using Manufactures.Helpers.PDFTemplates;
using Manufactures.Dtos.GarmentSubcon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Manufactures.Helpers.PDFTemplates.GarmentSubcon;

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("invoice-packing-list")]
    public class GarmentSubconInvoicePackingListController : ControllerApiBase
    {
        private readonly ISubconInvoicePackingListRepository subconInvoicePackingListRepository;
        private readonly ISubconInvoicePackingListItemRepository subconInvoicePackingListItemRepository;

        public GarmentSubconInvoicePackingListController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            subconInvoicePackingListRepository = Storage.GetRepository<ISubconInvoicePackingListRepository>();
            subconInvoicePackingListItemRepository = Storage.GetRepository<ISubconInvoicePackingListItemRepository>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")] List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = subconInvoicePackingListRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);
            //double totalQty = query.Sum(b => b.Quantity);
            List<GarmentSubconInvoicePackingListDto> garmentSubconInvoicePackingListDtos = subconInvoicePackingListRepository
                .Find(query)
                .Select(subconContract => new GarmentSubconInvoicePackingListDto(subconContract))
                .ToList();

            var dtoIds = garmentSubconInvoicePackingListDtos.Select(s => s.Id).ToList();

            var items = subconInvoicePackingListItemRepository.Query
                .Where(o => dtoIds.Contains(o.InvoicePackingListId))
                .Select(s => new { s.Identity,s.InvoicePackingListId,s.DLNo })
                .ToList();

            Parallel.ForEach(garmentSubconInvoicePackingListDtos, dto =>
            {
                var currentItems = items.Where(s => s.InvoicePackingListId == dto.Id);
                dto.DLNos = currentItems.Select(s => s.DLNo).ToList();
               
            });

            await Task.Yield();
            return Ok(garmentSubconInvoicePackingListDtos, info: new
            {
                page,
                size,
                total,
                //totalQty
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconInvoicePackingListCommand command)
        {
            try
            {
                VerifyUser();
                //var subcon = _garmentSubconContractRepository.Find(a => a.ContractNo.Replace(" ", "") == command.ContractNo.Replace(" ", "")).Select(o => new GarmentSubconContractDto(o)).FirstOrDefault();
                //if (subcon != null)
                //    return BadRequest(new
                //    {
                //        code = HttpStatusCode.BadRequest,
                //        error = "No/Tgl Contract sudah ada"
                //    });

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconInvoicePackingListDto garmentSubconInvoicePackingListDto  = subconInvoicePackingListRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconInvoicePackingListDto(subcon)
            {
                Items = subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == subcon.Identity).Select(subconItem => new GarmentSubconInvoicePackingListItemDto(subconItem)
                {

                }).ToList()
            }).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconInvoicePackingListDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconInvoicePackingListCommand command)
        {
            Guid guid = Guid.Parse(id);
            //var subcon = _garmentSubconContractRepository.Find(a => a.ContractNo.Replace(" ", "") == command.ContractNo.Replace(" ", "") && a.Identity!=command.Identity).Select(o => new GarmentSubconContractDto(o)).FirstOrDefault();
            //if (subcon != null)
            //    return BadRequest(new
            //    {
            //        code = HttpStatusCode.BadRequest,
            //        error = "No/Tgl Contract sudah ada"
            //    });
            command.SetIdentity(guid);

            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            RemoveGarmentSubconInvoicePackingListCommand command = new RemoveGarmentSubconInvoicePackingListCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSubconInvoicePackingListDto garmentSubconInvoicePackingListDto = subconInvoicePackingListRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconInvoicePackingListDto(subcon)
            {
                Items = subconInvoicePackingListItemRepository.Find(o => o.InvoicePackingListId == subcon.Identity).Select(subconItem => new GarmentSubconInvoicePackingListItemDto(subconItem)
                {
                  
                }).ToList()

            }
            ).FirstOrDefault();
            var stream = GarmentSubconInvoicePackingListPDFTemplate.Generate(garmentSubconInvoicePackingListDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSubconInvoicePackingListDto.InvoiceNo}.pdf"
            };
        }
    }
}
