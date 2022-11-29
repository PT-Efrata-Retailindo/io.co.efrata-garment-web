using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.ReadModels
{
    public class GarmentCuttingOutReadModel : ReadModelBase
    {
        public GarmentCuttingOutReadModel(Guid identity) : base(identity)
        {
        }

        public string CutOutNo { get; internal set; }
        public string CuttingOutType { get; internal set; }
        public int UnitFromId { get; internal set; }
        public string UnitFromCode { get; internal set; }
        public string UnitFromName { get; internal set; }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
        public string RONo { get; internal set; }
        public string Article { get; internal set; }
        public int ComodityId { get; internal set; }
        public string ComodityCode { get; internal set; }
        public string ComodityName { get; internal set; }
        public DateTimeOffset CuttingOutDate { get; internal set; }
        public long EPOId { get; internal set; }
        public long EPOItemId { get; internal set; }
        public string POSerialNumber { get; internal set; }
		public string UId { get; private set; }
        public bool IsUsed { get; internal set; }
        public virtual List<GarmentCuttingOutItemReadModel> GarmentCuttingOutItem { get; internal set; }
    }
}
