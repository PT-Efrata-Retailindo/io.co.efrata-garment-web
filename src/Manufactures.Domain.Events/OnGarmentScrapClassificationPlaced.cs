using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapClassificationPlaced : IGarmentScrapClassificationEvent
	{
		public OnGarmentScrapClassificationPlaced(Guid garmentScrapClassificationId)
		{
			GarmentScrapClassificationId = garmentScrapClassificationId;
		}
		public Guid GarmentScrapClassificationId { get; }
	}
}
