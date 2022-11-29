using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapClassifications.CommandHandler
{
	public class PlaceGarmentScrapClassificationCommandHandler : ICommandHandler<PlaceGarmentScrapClassificationCommand, GarmentScrapClassification>
	{
		private readonly IStorage _storage;

		private readonly IGarmentScrapClassificationRepository _garmentScrapClassificationRepository;
		
		public PlaceGarmentScrapClassificationCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapClassificationRepository = storage.GetRepository<IGarmentScrapClassificationRepository>();
		}

		public async Task<GarmentScrapClassification> Handle(PlaceGarmentScrapClassificationCommand request, CancellationToken cancellationToken)
		{

			GarmentScrapClassification garmentScrapClassification = new GarmentScrapClassification(
				Guid.NewGuid(),
				request.Code,
				request.Name,
				request.Description 
			);

			
			await _garmentScrapClassificationRepository.Update(garmentScrapClassification);

			_storage.Save();

			return garmentScrapClassification;
		}

	}
}
