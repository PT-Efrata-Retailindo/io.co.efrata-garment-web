using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleRequest.CommandHandler
{
    public class ReceivedGarmentSampleRequestCommandHandler : ICommandHandler<ReceivedGarmentSampleRequestCommand, GarmentSampleRequest>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleRequestRepository _GarmentSampleRequestRepository;

        public ReceivedGarmentSampleRequestCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
        }

        public async Task<GarmentSampleRequest> Handle(ReceivedGarmentSampleRequestCommand request, CancellationToken cancellationToken)
        {
            var SampleRequest = _GarmentSampleRequestRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleRequest(o)).Single();

            SampleRequest.SetIsReceived(request.IsReceived);
            SampleRequest.SetReceivedDate(request.ReceivedDate);
            SampleRequest.SetReceivedBy(request.ReceivedBy);

            SampleRequest.Modify();

            await _GarmentSampleRequestRepository.Update(SampleRequest);

            _storage.Save();

            return SampleRequest;
        }
    }
}
