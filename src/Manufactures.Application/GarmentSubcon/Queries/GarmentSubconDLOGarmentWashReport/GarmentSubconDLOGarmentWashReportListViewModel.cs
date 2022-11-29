using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport
{
    public class GarmentSubconDLOGarmentWashReportListViewModel
    {
        public List<GarmentSubconDLOGarmentWashReportDto> garmentSubconDLOGarmentWashReportDto { get; set; }
        public int count { get; set; }       
    }
}
