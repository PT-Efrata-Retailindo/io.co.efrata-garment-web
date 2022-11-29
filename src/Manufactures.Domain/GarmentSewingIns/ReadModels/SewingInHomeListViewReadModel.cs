using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.ReadModels
{
    public class SewingInHomeListViewReadModel : ReadModelBase
    {
        //Enhance Jason Aug 2021
        public SewingInHomeListViewReadModel(Guid identity) : base(identity)
        {
        }

        public string SewingInNo { get; set; }
        public string Article { get; set; }
        public double TotalQuantity { get; set; }
        public double TotalRemainingQuantity { get; set; }
        public string RONo { get; set; }
        public string UnitFromCode { get; internal set; }
        public string UnitCode { get; internal set; }
        public string SewingFrom { get; set; }
        public DateTimeOffset SewingInDate { get; set; }
        public string Products { get; set; }
    }
}