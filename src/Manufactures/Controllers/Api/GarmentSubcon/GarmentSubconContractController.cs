using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.ExcelTemplates;
using Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
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

namespace Manufactures.Controllers.Api.GarmentSubcon
{
    [ApiController]
    [Authorize]
    [Route("subcon-contracts")]
    public class GarmentSubconContractController : ControllerApiBase
    {
        private readonly IGarmentSubconContractRepository _garmentSubconContractRepository;
        private readonly IGarmentSubconContractItemRepository _garmentSubconContractItemRepository;

        public GarmentSubconContractController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _garmentSubconContractRepository = Storage.GetRepository<IGarmentSubconContractRepository>();
            _garmentSubconContractItemRepository = Storage.GetRepository<IGarmentSubconContractItemRepository>();
        }
        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconContractRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            query = query.Skip((page - 1) * size).Take(size);
            double totalQty = query.Sum(b => b.Quantity);
            List<GarmentSubconContractDto> garmentSubconContractListDtos = _garmentSubconContractRepository
                .Find(query)
                .Select(subconContract => new GarmentSubconContractDto(subconContract))
                .ToList();

            var dtoIds = garmentSubconContractListDtos.Select(s => s.Id).ToList();
            await Task.Yield();
            return Ok(garmentSubconContractListDtos, info: new
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

            GarmentSubconContractDto garmentSubconContractDto = _garmentSubconContractRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconContractDto(subcon)
            {
                Items = _garmentSubconContractItemRepository.Find(o => o.SubconContractId == subcon.Identity).Select(subconItem => new GarmentSubconContractItemDto(subconItem)
                {

                }).ToList()
            }).FirstOrDefault();

            await Task.Yield();
            return Ok(garmentSubconContractDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSubconContractCommand command)
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSubconContractCommand command)
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

            RemoveGarmentSubconContractCommand command = new RemoveGarmentSubconContractCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query = _garmentSubconContractRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSubconContractDto = _garmentSubconContractRepository.Find(query).Select(o => new GarmentSubconContractDto(o)).ToArray();
            var garmentSubconContractItemDto = _garmentSubconContractItemRepository.Find(_garmentSubconContractItemRepository.Query).Select(o => new GarmentSubconContractItemDto(o)).ToList();

            Parallel.ForEach(garmentSubconContractDto, itemDto =>
            {
                var garmentSubconContractItems = garmentSubconContractItemDto.Where(x => x.SubconContractId == itemDto.Id).OrderBy(x => x.Id).ToList();

                itemDto.Items = garmentSubconContractItems;

            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSubconContractDto = QueryHelper<GarmentSubconContractDto>.Order(garmentSubconContractDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSubconContractDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpGet("realization-report")]
        public async Task<IActionResult> GetReaizationReport(string subconcontractNo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GarmentRealizationSubconReportQuery query = new GarmentRealizationSubconReportQuery(page, size, Order, subconcontractNo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            var model = new { IN = viewModel.garmentRealizationSubconReportDtos, Out = viewModel.garmentRealizationSubconReportDtosOUT };

            return Ok(model, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("realization-report/download")]
        public async Task<IActionResult> GetXlsReaizationReport(string subconcontractNo, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsGarmentRealizationSubconReportQuery query = new GetXlsGarmentRealizationSubconReportQuery(page, size, Order, subconcontractNo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Realisasi Subcon ";

                if (subconcontractNo != null) filename += " " + subconcontractNo;
                filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("subcon-contract-report")]
        public async Task<IActionResult> GetSubconContractReport(int supplierNo, string contractType, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            VerifyUser();
            GarmentSubconContactReportQuery query = new GarmentSubconContactReportQuery(page, size,order, supplierNo, contractType, dateFrom, dateTo);
            var viewModel = await Mediator.Send(query);

           

            return Ok(viewModel, info: new
            {
                page,
                size,
                viewModel.count
            });
           
        }

        [HttpGet("subcon-contract-report/download")]
        public async Task<IActionResult> GetXlsSubconContractReport(int supplierNo, string contractType, DateTime dateFrom, DateTime dateTo, int page = 1, int size = 25, string order = "{ }")
        {
            try
            {
                VerifyUser();
                GetXlsGarmentSubconContractReporQuery query = new GetXlsGarmentSubconContractReporQuery(page, size, order, supplierNo, contractType, dateFrom, dateTo);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = String.Format("Laporan SubContract.xlsx");

                //if (subconcontractNo != null) filename += " " + subconcontractNo;
                //filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet("xls/{id}")]
        public async Task<IActionResult> GetXls(string id)
        {
            try
            {
                Guid guid = Guid.Parse(id);
                VerifyUser();

                GarmentSubconContractExcelDto garmentSubconContractDto = _garmentSubconContractRepository.Find(o => o.Identity == guid).Select(subcon => new GarmentSubconContractExcelDto(subcon)
                {
                    Items = _garmentSubconContractItemRepository.Find(o => o.SubconContractId == subcon.Identity).Select(subconItem => new GarmentSubconContractItemExcelDto(subconItem)
                    {

                    }).ToList()
                }).FirstOrDefault();
                byte[] xlsInBytes;

                var xls = GarmentSubconContractAgreementExcelTemplate.GenerateExcelTemplate(garmentSubconContractDto);

                string filename = String.Format($"{garmentSubconContractDto.ContractNo}.xlsx");

                //if (subconcontractNo != null) filename += " " + subconcontractNo;
                //filename += ".xlsx";

                xlsInBytes = xls.ToArray();
                var file = File(xlsInBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                return file;
            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
