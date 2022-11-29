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

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class UpdateGarmentSampleRequestCommandHandler : ICommandHandler<UpdateGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;
        private readonly IGarmentSampleRequestProductRepository _GarmentSampleRequestProductRepository;
        private readonly IGarmentSampleRequestSpecificationRepository _garmentSampleRequestSpecificationRepository;
        private readonly IAzureImage _azureImage;
        private readonly IAzureDocument _azureDocument;

        public UpdateGarmentSampleRequestCommandHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
            _GarmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
            _garmentSampleRequestSpecificationRepository = storage.GetRepository<IGarmentSampleRequestSpecificationRepository>();
            _azureImage = serviceProvider.GetService<IAzureImage>();
            _azureDocument = serviceProvider.GetService<IAzureDocument>();
        }

        public async Task<GarmentSampleRequest> Handle(UpdateGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            var SampleRequest = _GarmentSampleRequestRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleRequest(o)).Single();

            await _azureImage.RemoveMultipleImage("GarmentSampleRequest", SampleRequest.ImagesPath);
            await _azureDocument.RemoveMultipleFile("GarmentSampleRequest", SampleRequest.DocumentsPath);

            _GarmentSampleRequestProductRepository.Find(o => o.SampleRequestId == SampleRequest.Identity).ForEach(async product =>
            {
                var item = request.SampleProducts.Where(o => o.Id == product.Identity).SingleOrDefault();

                if (item == null)
                {
                    product.Remove();
                }
                else
                {
                    product.SetColor(item.Color);
                    product.SetFabric(item.Fabric);
                    product.SetQuantity(item.Quantity);
                    product.SetSizeDescription(item.SizeDescription);
                    product.SetSizeId(new SizeId(item.Size.Id));
                    product.SetSizeName(item.Size.Size);
                    product.SetStyle(item.Style);
                    product.Modify();
                }


                await _GarmentSampleRequestProductRepository.Update(product);
            });

            foreach (var product in request.SampleProducts)
            {
                if (product.Id == Guid.Empty)
                {
                    GarmentSampleRequestProduct GarmentSampleRequestProduct = new GarmentSampleRequestProduct(
                        Guid.NewGuid(),
                        SampleRequest.Identity,
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
            }

            _garmentSampleRequestSpecificationRepository.Find(o => o.SampleRequestId == SampleRequest.Identity).ForEach(async spec =>
            {
                var specification = request.SampleSpecifications.Where(o => o.Id == spec.Identity).SingleOrDefault();

                if (specification == null)
                {
                    spec.Remove();
                }
                else
                {
                    spec.SetQuantity(specification.Quantity);
                    spec.SetInventory(specification.Inventory);
                    spec.SetRemark(specification.Remark);
                    spec.SetSpecificationDetail(specification.SpecificationDetail);
                    spec.SetUomId(new UomId(specification.Uom.Id));
                    spec.SetUomUnit(specification.Uom.Unit);
                    spec.Modify();
                }

                await _garmentSampleRequestSpecificationRepository.Update(spec);
            });

            foreach (var specification in request.SampleSpecifications)
            {
                if (specification.Id == Guid.Empty)
                {
                    GarmentSampleRequestSpecification GarmentSampleRequestSpecification = new GarmentSampleRequestSpecification(
                        Guid.NewGuid(),
                        SampleRequest.Identity,
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
            }


            SampleRequest.SetRemark(request.Remark);
            SampleRequest.SetBuyerId(new BuyerId(request.Buyer.Id));
            SampleRequest.SetBuyerCode(request.Buyer.Code);
            SampleRequest.SetBuyerName(request.Buyer.Name);
            SampleRequest.SetSentDate(request.SentDate);
            SampleRequest.SetSampleType(request.SampleType);
            SampleRequest.SetDate(request.Date);
            SampleRequest.SetComodityId(new GarmentComodityId(request.Comodity.Id));
            SampleRequest.SetComodityCode(request.Comodity.Code);
            SampleRequest.SetComodityName(request.Comodity.Name);
            SampleRequest.SetPacking(request.Packing);
            SampleRequest.SetPOBuyer(request.POBuyer);
            SampleRequest.SetRONoCC(request.RONoCC);
            SampleRequest.SetAttached(request.Attached);
            SampleRequest.SetSectionCode(request.Section.Code);
            SampleRequest.SetSectionId(new SectionId(request.Section.Id));
            SampleRequest.SetImagesName(request.ImagesName);
            SampleRequest.SetDocumentsFileName(request.DocumentsFileName);
            SampleRequest.SetImagesPath(await _azureImage.UploadMultipleImage("GarmentSampleRequest", SampleRequest.Identity, DateTime.UtcNow, request.ImagesFile, request.ImagesPath));
            SampleRequest.SetDocumentsPath(await _azureDocument.UploadMultipleFile("GarmentSampleRequest", SampleRequest.Identity, DateTime.UtcNow, request.DocumentsFile, request.DocumentsFileName, request.DocumentsPath));
            SampleRequest.SetSampleTo(request.SampleTo);
            SampleRequest.Modify();

            await _GarmentSampleRequestRepository.Update(SampleRequest);

            _storage.Save();

            return SampleRequest;
        }

    }
}
