using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts
{
    public class GarmentMonitoringSampleFinishingReportDataTemplate
    {
        public string RoJob { get; set; }
        public string Article { get; set; }
        public string BuyerCode { get; set; }
        public int UnitId { get; set; }
        public double QtyOrder { get; set; }
        public string Style { get; set; }
        public decimal Price { get; set; }
        public double Stock { get; set; }
        public double SewingQtyPcs { get; set; }
        public double FinishingQtyPcs { get; set; }
        public double RemainQty { get; set; }
        public decimal Nominal { get; set; }
        public string UomUnit { get; set; }
    }
}
