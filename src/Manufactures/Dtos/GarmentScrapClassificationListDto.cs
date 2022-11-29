using Manufactures.Domain.GarmentScrapClassifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
	class GarmentScrapClassificationListDto : BaseDto
	{
		public GarmentScrapClassificationListDto(GarmentScrapClassification garmentScrapClassification)
		{
			Id = garmentScrapClassification.Identity;
			Code = garmentScrapClassification.Code;
			Name = garmentScrapClassification.Name;
			Description = garmentScrapClassification.Description ;
		}

		public Guid Id { get; internal set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; }
	}
}
