using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentSewingDOs.ReadModels
{
    public class GarmentSewingDOReadModel : ReadModelBase
    {
        public GarmentSewingDOReadModel(Guid identity): base(identity)
        {
        }

        public string SewingDONo { get; internal set; }
        public Guid CuttingOutId { get; internal set; }
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
        public DateTimeOffset SewingDODate { get; internal set; }
		public string UId { get; private set; }
		public virtual List<GarmentSewingDOItemReadModel> GarmentSewingDOItem { get; internal set; }
    }
}
