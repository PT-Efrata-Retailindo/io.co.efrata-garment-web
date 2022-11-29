using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.MonitoringProductionFlow
{
	public interface IGarmentMonitoringProductionFlowRepository : IAggregateRepository<GarmentMonitoringProductionFlow, GarmentMonitoringProductionFlowReadModel>
	{
		IQueryable<GarmentMonitoringProductionFlowReadModel> Read(string order, List<string> select, string filter,string keyword);
	}
}
