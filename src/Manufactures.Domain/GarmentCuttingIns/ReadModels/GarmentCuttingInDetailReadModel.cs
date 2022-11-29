using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.ReadModels
{
    public class GarmentCuttingInDetailReadModel : ReadModelBase
    {
        public GarmentCuttingInDetailReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutInItemId { get; internal set; }
        public Guid PreparingItemId { get; internal set; }

        public Guid SewingOutItemId { get; internal set; }
        public Guid SewingOutDetailId { get; internal set; }

        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }

        public string DesignColor { get; internal set; }
        public string FabricType { get; internal set; }

        public double PreparingQuantity { get; internal set; }
        public int PreparingUomId { get; internal set; }
        public string PreparingUomUnit { get; internal set; }

        public int CuttingInQuantity { get; internal set; }
        public int CuttingInUomId { get; internal set; }
        public string CuttingInUomUnit { get; internal set; }

        public double RemainingQuantity { get; internal set; }
        public double BasicPrice { get; internal set; }
        public double Price { get; internal set; }
        public double FC { get; internal set; }

        public string Color { get; internal set; }
		public string UId { get; private set; }
		public virtual GarmentCuttingInItemReadModel GarmentCuttingInItem { get; internal set; }
    }
}
