using Barebone.Controllers;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample;
using Manufactures.Application.AzureUtility;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using Manufactures.Helpers.PDFTemplates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Manufactures.Controllers.Api.GarmentSample
{
    [ApiController]
    [Authorize]
    [Route("garment-sample-requests")]
    public class GarmentSampleRequestController: ControllerApiBase
    {
        
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository _GarmentSampleRequestProductRepository;
        private readonly IGarmentSampleRequestSpecificationRepository _GarmentSampleRequestSpecificationRepository;
        private readonly IAzureImage _azureImage;
        private readonly IAzureDocument _azureDocument;

        public GarmentSampleRequestController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _GarmentSampleRequestRepository = Storage.GetRepository<IGarmentSampleRequestRepository>();
            _GarmentSampleRequestProductRepository = Storage.GetRepository<IGarmentSampleRequestProductRepository>();
            _GarmentSampleRequestSpecificationRepository = Storage.GetRepository<IGarmentSampleRequestSpecificationRepository>();
            _azureImage = serviceProvider.GetService<IAzureImage>();
            _azureDocument = serviceProvider.GetService<IAzureDocument>();
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query =_GarmentSampleRequestRepository.Read(page, size, order, keyword, filter);
            var total = query.Count();
            
            query = query.Skip((page - 1) * size).Take(size);

            List<GarmentSampleRequestListDto> garmentSampleRequestListDtos =_GarmentSampleRequestRepository
                .Find(query)
                .Select(subcon => new GarmentSampleRequestListDto(subcon))
                .ToList();

            var dtoIds = garmentSampleRequestListDtos.Select(s => s.Id).ToList();
            

            await Task.Yield();
            return Ok(garmentSampleRequestListDtos, info: new
            {
                page,
                size,
                total
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();

            GarmentSampleRequestDto garmentSampleRequestDto =_GarmentSampleRequestRepository.Find(o => o.Identity == guid).Select(sample => new GarmentSampleRequestDto(sample)
            {
                SampleProducts = _GarmentSampleRequestProductRepository.Find(o => o.SampleRequestId == sample.Identity).Select(product => new GarmentSampleRequestProductDto(product)
                {
                   
                }).OrderBy(o => o.Index).ToList(),
                SampleSpecifications= _GarmentSampleRequestSpecificationRepository.Find(o => o.SampleRequestId == sample.Identity).Select(specification => new GarmentSampleRequestSpecificationDto(specification)
                {

                }).OrderBy(o => o.Index).ToList()
            }
            ).FirstOrDefault();
            if (garmentSampleRequestDto.ImagesPath.Count > 0)
            {
                garmentSampleRequestDto.ImagesFile = await _azureImage.DownloadMultipleImages("GarmentSampleRequest", garmentSampleRequestDto.ImagesPath);
            }
            if (garmentSampleRequestDto.DocumentsPath.Count > 0)
            {
                garmentSampleRequestDto.DocumentsFile = await _azureDocument.DownloadMultipleFiles("GarmentSampleRequest", garmentSampleRequestDto.DocumentsPath);
            }

            await Task.Yield();
            return Ok(garmentSampleRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PlaceGarmentSampleRequestCommand command)
        {
            try
            {
                VerifyUser();

                var order = await Mediator.Send(command);

                return Ok(order.Identity);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet("complete")]
        public async Task<IActionResult> GetComplete(int page = 1, int size = 25, string order = "{}", [Bind(Prefix = "Select[]")]List<string> select = null, string keyword = null, string filter = "{}")
        {
            VerifyUser();

            var query =_GarmentSampleRequestRepository.Read(page, size, order, keyword, filter);
            var count = query.Count();

            var garmentSampleRequestDto =_GarmentSampleRequestRepository.Find(query).Select(o => new GarmentSampleRequestDto(o)).ToArray();
            var GarmentSampleRequestProductDto = _GarmentSampleRequestProductRepository.Find(_GarmentSampleRequestProductRepository.Query).Select(o => new GarmentSampleRequestProductDto(o)).ToList();
            var GarmentSampleRequestSpecificationDto = _GarmentSampleRequestSpecificationRepository.Find(_GarmentSampleRequestSpecificationRepository.Query).Select(o => new GarmentSampleRequestSpecificationDto(o)).ToList();
            
            Parallel.ForEach(garmentSampleRequestDto, productDto =>
            {
                var GarmentSampleRequestProducts = GarmentSampleRequestProductDto.Where(x => x.SampleRequestId == productDto.Id).OrderBy(x => x.Id).ToList();
                var GarmentSampleSpecifications = GarmentSampleRequestSpecificationDto.Where(x => x.SampleRequestId == productDto.Id).OrderBy(x => x.Id).ToList();

                productDto.SampleProducts = GarmentSampleRequestProducts;
                productDto.SampleSpecifications = GarmentSampleSpecifications;
            });

            if (order != "{}")
            {
                Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                garmentSampleRequestDto = QueryHelper<GarmentSampleRequestDto>.Order(garmentSampleRequestDto.AsQueryable(), OrderDictionary).ToArray();
            }

            await Task.Yield();
            return Ok(garmentSampleRequestDto, info: new
            {
                page,
                size,
                count
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] UpdateGarmentSampleRequestCommand command)
        {
            Guid guid = Guid.Parse(id);

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

            RemoveGarmentSampleRequestCommand command = new RemoveGarmentSampleRequestCommand(guid);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);

        }

        [HttpPut("post")]
        public async Task<IActionResult> postData([FromBody]PostGarmentSampleRequestCommand command)
        {
            VerifyUser();

            var order = await Mediator.Send(command);

            return Ok();
        }

        [HttpPut("received/{id}")]
        public async Task<IActionResult> receivedData(string id, [FromBody] ReceivedGarmentSampleRequestCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var username = WorkContext.UserName;
            command.ReceivedDate = DateTimeOffset.Now;
            command.ReceivedBy = username;

            var order = await Mediator.Send(command);

            return Ok();
        }

        [HttpGet("get-pdf/{id}")]
        public async Task<IActionResult> GetPdf(string id)
        {
            Guid guid = Guid.Parse(id);

            VerifyUser();
            
            GarmentSampleRequestDto garmentSampleRequestDto = _GarmentSampleRequestRepository.Find(o => o.Identity == guid).Select(sample => new GarmentSampleRequestDto(sample)
            {
                SampleProducts = _GarmentSampleRequestProductRepository.Find(o => o.SampleRequestId == sample.Identity).Select(product => new GarmentSampleRequestProductDto(product)).OrderBy(o => o.Index).ToList(),
                SampleSpecifications = _GarmentSampleRequestSpecificationRepository.Find(o => o.SampleRequestId == sample.Identity).Select(specification => new GarmentSampleRequestSpecificationDto(specification)).OrderBy(o => o.Index).ToList()
            }
            ).FirstOrDefault();
            if (garmentSampleRequestDto.ImagesPath.Count > 0)
            {
                garmentSampleRequestDto.ImagesFile = await _azureImage.DownloadMultipleImages("GarmentSampleRequest", garmentSampleRequestDto.ImagesPath);
            }
            var stream = GarmentSampleRequestPDFTemplate.Generate(garmentSampleRequestDto);

            return new FileStreamResult(stream, "application/pdf")
            {
                FileDownloadName = $"{garmentSampleRequestDto.SampleRequestNo}.pdf"
            };
        }

        [HttpPut("rejected/{id}")]
        public async Task<IActionResult> rejectedData(string id, [FromBody] RejectGarmentSampleRequestCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var username = WorkContext.UserName;
            command.RejectedDate = DateTimeOffset.Now;
            command.RejectedBy = username;

            var order = await Mediator.Send(command);

            return Ok();
        }

        [HttpPut("revised/{id}")]
        public async Task<IActionResult> revisedData(string id, [FromBody] RevisedGarmentSampleRequestCommand command)
        {
            Guid guid = Guid.Parse(id);

            command.SetIdentity(guid);

            VerifyUser();

            var username = WorkContext.UserName;
            command.RevisedDate = DateTimeOffset.Now;
            command.RevisedBy = username;

            var order = await Mediator.Send(command);

            return Ok();
        }

        [HttpGet("monitoring")]
        public async Task<IActionResult> GetMonitoring( DateTime receivedDateFrom, DateTime receivedDateTo, int page = 1, int size = 25, string Order = "{}")
        {
            VerifyUser();
            GetMonitoringReceiptSampleQuery query = new GetMonitoringReceiptSampleQuery(page, size, Order, receivedDateFrom, receivedDateTo, WorkContext.Token);
            var viewModel = await Mediator.Send(query);

            return Ok(viewModel.garmentMonitorings, info: new
            {
                page,
                size,
                viewModel.count
            });
        }

        [HttpGet("download")]
        public async Task<IActionResult> GetXls(int unit, DateTime receivedDateFrom, DateTime receivedDateTo, string type, int page = 1, int size = 25, string Order = "{}")
        {
            try
            {
                VerifyUser();
                GetXlsReceiptSampleQuery query = new GetXlsReceiptSampleQuery(page, size, Order, receivedDateFrom, receivedDateTo, WorkContext.Token);
                byte[] xlsInBytes;

                var xls = await Mediator.Send(query);

                string filename = "Laporan Penerimaan Sample";

                if (receivedDateFrom != null) filename += " " + ((DateTime)receivedDateFrom).ToString("dd-MM-yyyy");

                if (receivedDateTo != null) filename += "_" + ((DateTime)receivedDateTo).ToString("dd-MM-yyyy");
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
    }
}
