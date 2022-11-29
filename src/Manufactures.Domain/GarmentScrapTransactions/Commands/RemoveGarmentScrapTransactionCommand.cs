using Infrastructure.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Commands
{
	public class RemoveGarmentScrapTransactionCommand : ICommand<GarmentScrapTransaction>
	{
		public RemoveGarmentScrapTransactionCommand(Guid id)
		{
			Identity = id;
		}

		public Guid Identity { get; private set; }
	}
}
