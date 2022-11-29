using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.AzureUtility;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class PlaceGarmentSampleRequestCommandHandler : ICommandHandler<PlaceGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository _GarmentSampleRequestProductRepository;
        private readonly IGarmentSampleRequestSpecificationRepository _garmentSampleRequestSpecificationRepository;
        public IAzureImage _azureImage;
        public IAzureDocument _azureDocument;

        public PlaceGarmentSampleRequestCommandHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            _garmentSampleRequestSpecificationRepository = storage.GetRepository<IGarmentSampleRequestSpecificationRepository>();
            _azureImage = serviceProvider.GetService<IAzureImage>();
            _azureDocument = serviceProvider.GetService<IAzureDocument>();
        }

        public async Task<GarmentSampleRequest> Handle(PlaceGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            request.SampleProducts = request.SampleProducts.ToList();
            request.SampleSpecifications = request.SampleSpecifications.ToList();
            var id = Guid.NewGuid();
            GarmentSampleRequest GarmentSampleRequest = new GarmentSampleRequest(
                id,
                request.SampleCategory,
                //GenerateSampleRequestNo(request),
                "",
                GenerateROSample(request),
                request.RONoCC,
                request.Date,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.SampleType,
                request.Packing,
                request.SentDate,
                request.POBuyer,
                request.Attached,
                request.Remark,
                request.IsPosted,
                request.IsReceived,
                request.ReceivedDate,
                request.ReceivedBy,
                request.IsRejected,
                request.RejectedDate,
                request.RejectedBy,
                request.RejectedReason,
                request.IsRevised,
                request.RevisedDate,
                request.RevisedBy,
                request.RevisedReason,
                JsonConvert.SerializeObject(request.ImagesPath),
                JsonConvert.SerializeObject(request.DocumentsPath),
                JsonConvert.SerializeObject(request.ImagesName),
                JsonConvert.SerializeObject(request.DocumentsFileName),
                new SectionId(request.Section.Id),
                request.Section.Code,
                request.SampleTo
            );

            foreach (var product in request.SampleProducts)
            {
                GarmentSampleRequestProduct GarmentSampleRequestProduct = new GarmentSampleRequestProduct(
                    Guid.NewGuid(),
                    GarmentSampleRequest.Identity,
                    product.Style,
                    product.Color,
                    product.Fabric,
                    new SizeId(product.Size.Id),
                    product.Size.Size,
                    product.SizeDescription,
                    product.Quantity,
                    product.Index
                );

                await _GarmentSampleRequestProductRepository.Update(GarmentSampleRequestProduct);
            }



            foreach (var specification in request.SampleSpecifications)
            {

                GarmentSampleRequestSpecification GarmentSampleRequestSpecification = new GarmentSampleRequestSpecification(
                    Guid.NewGuid(),
                    GarmentSampleRequest.Identity,
                    specification.Inventory,
                    specification.SpecificationDetail,
                    specification.Quantity,
                    specification.Remark,
                    new UomId(specification.Uom.Id),
                    specification.Uom.Unit,
                    specification.Index
                );

                await _garmentSampleRequestSpecificationRepository.Update(GarmentSampleRequestSpecification);
            }

            await _GarmentSampleRequestRepository.Update(GarmentSampleRequest);

            _storage.Save();

            var SampleRequest = _GarmentSampleRequestRepository.Query.Where(o => o.Identity == id).Select(o => new GarmentSampleRequest(o)).Single();
            if(request.ImagesFile.Count > 0)
            {
                SampleRequest.SetImagesPath(await _azureImage.UploadMultipleImage(GarmentSampleRequest.GetType().Name, id, SampleRequest.AuditTrail.CreatedDate.UtcDateTime, request.ImagesFile, request.ImagesPath));
            } else
            {
                SampleRequest.SetImagesPath("[]");
            }
            if(request.DocumentsFile.Count > 0)
            {
                SampleRequest.SetDocumentsPath(await _azureDocument.UploadMultipleFile(GarmentSampleRequest.GetType().Name, id, SampleRequest.AuditTrail.CreatedDate.UtcDateTime, request.DocumentsFile, request.DocumentsFileName, request.DocumentsPath));
            } else
            {
                SampleRequest.SetDocumentsPath("[]");
            }
            
            SampleRequest.Modify();
            await _GarmentSampleRequestRepository.Update(SampleRequest);

            _storage.Save();

            return SampleRequest;
        }


        private string GenerateROSample(PlaceGarmentSampleRequestCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SampleCategory=="Commercial Sample" ? "CS" : "NCS";

            var prefix = $"{year}";

            var lastRONo = _GarmentSampleRequestRepository.Query.Where(w => w.RONoSample.StartsWith(prefix) && w.RONoSample.EndsWith(code))
                .OrderByDescending(o => o.RONoSample)
                .Select(s => int.Parse(s.RONoSample.Substring(2,5)))
                .FirstOrDefault();
            var RONo = $"{prefix}{(lastRONo + 1).ToString("D5")}{code}";

            return RONo;
        }
    }
}

