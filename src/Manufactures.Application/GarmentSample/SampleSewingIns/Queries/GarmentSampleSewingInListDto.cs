using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SampleSewingIns.Queries
{
    public class GarmentSampleSewingInListDto
    {
        public Guid Id { get; set; }
        public string SewingInNo { get; set; }
        public string SewingFrom { get; set; }
        public Guid CuttingOutId { get; set; }
        public string CuttingOutNo { get; set; }
        public UnitDepartment UnitFrom { get; set; }
        public UnitDepartment Unit { get; set; }
        public string RONo { get; set; }
        public string Article { get; set; }
        public GarmentComodity Comodity { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public List<string> Products { get; set; }
        public List<GarmentSampleSewingInItemDto> Items { get; set; }
    }
}
