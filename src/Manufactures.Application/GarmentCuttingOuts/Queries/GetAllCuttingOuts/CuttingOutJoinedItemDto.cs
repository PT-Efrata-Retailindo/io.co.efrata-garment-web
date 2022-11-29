using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts
{
    class CuttingOutJoinedItemDto
    {
        public Guid Id { get; internal set; }
        public string CutOutNo { get; internal set; }
        public string CuttingOutType { get; internal set; }

        public int UnitFromId { get; internal set; }
        public object UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public DateTimeOffset CuttingOutDate { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }

        public Guid ItemId { get; internal set; }
        public Guid CutOutId { get; internal set; }
        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double TotalCuttingOut { get; internal set; }
    }
}
