using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapClassifications.CommandHandler
{
	public class RemoveGarmentScrapClassificationCommandHandler : ICommandHandler<RemoveGarmentScrapClassificationCommand, GarmentScrapClassification>
	{
		private readonly IStorage _storage;

		private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;

		public RemoveGarmentScrapClassificationCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapClassificationRepository = storage.GetRepository<IGarmentScrapClassificationRepository>();
		}

		public async Task<GarmentScrapClassification> Handle(RemoveGarmentScrapClassificationCommand request, CancellationToken cancellationToken)
		{
			var gscrapClassification = _garmentScrapClassificationRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapClassification(o)).Single();

			if (gscrapClassification == null)
				throw Validator.ErrorValidation(("Identity", "Invalid Id: " + request.Identity));

			gscrapClassification.Remove();
			await _garmentScrapClassificationRepository.Update(gscrapClassification);
			_storage.Save();
			return gscrapClassification;
		}
	}
}
