using Infrastructure.Domain;
using Manufactures.Domain.Events.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport
{
    public class GarmentSubconGarmentWashReportListViewModel
    {
        public List<GarmentSubconGarmentWashReportDto> garmentSubconGarmentWashReportDto { get; set; }
        public int count { get; set; }       
    }
}
