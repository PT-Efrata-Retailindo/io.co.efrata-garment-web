using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapClassifications.Commands
{
	public class RemoveGarmentScrapClassificationCommand : ICommand<GarmentScrapClassification>
	{
		public RemoveGarmentScrapClassificationCommand(Guid id)
		{
			Identity = id;
		}

		public Guid Identity { get; private set; }
	}
}
