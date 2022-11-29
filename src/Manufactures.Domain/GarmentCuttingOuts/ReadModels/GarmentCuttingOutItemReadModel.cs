using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ReadModels
{
    public class GarmentCuttingOutItemReadModel : ReadModelBase
    {
        public GarmentCuttingOutItemReadModel(Guid identity) : base(identity)
        {
        }

        public Guid CutOutId { get; internal set; }
        public Guid CuttingInId { get; internal set; }
        public Guid CuttingInDetailId { get; internal set; }
        public int ProductId { get; internal set; }
        public string ProductCode { get; internal set; }
        public string ProductName { get; internal set; }
        public string DesignColor { get; internal set; }
        public double TotalCuttingOut { get; internal set; }
		public string UId { get; private set; }

		public virtual ICollection<GarmentCuttingOutDetailReadModel> GarmentCuttingOutDetail { get; internal set; }
        public virtual GarmentCuttingOutReadModel GarmentCuttingOutIdentity { get; internal set; }
    }
}
