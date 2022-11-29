using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample
{
    public class GarmentMonitoringReceiptSampleViewModel
    {
        public List<GarmentMonitoringReceiptSampleDto> garmentMonitorings { get; set; }
        public int count { get; set; }
    }
}
