using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries
{
    public class ServiceSubconFabricWashDto
    {
        public ServiceSubconFabricWashDto()
        {
        }

        public string serviceSubconFabricWashNo { get; internal set; }
        public DateTimeOffset serviceSubconFabricWashDate { get; internal set; }
        public string unitExpenditureNo { get; internal set; }
        public DateTimeOffset expendituredate { get; internal set; }
        public string unitSenderCode { get; internal set; }
        public string unitSenderName { get; internal set; }
        public string productCode { get; internal set; }
        public string productName { get; internal set; }
        public string productRemark { get; internal set; }
        public string designcolor { get; internal set; }
        public decimal quantity { get; internal set; }
        public string uomUnit { get; internal set; }
        public ServiceSubconFabricWashDto(ServiceSubconFabricWashDto serviceSubconFabricWashDto)
        {
            serviceSubconFabricWashNo = serviceSubconFabricWashDto.serviceSubconFabricWashNo;
            serviceSubconFabricWashDate = serviceSubconFabricWashDto.serviceSubconFabricWashDate;
            unitExpenditureNo = serviceSubconFabricWashDto.unitExpenditureNo;
            expendituredate = serviceSubconFabricWashDto.expendituredate;
            unitSenderCode = serviceSubconFabricWashDto.unitSenderCode;
            unitSenderName = serviceSubconFabricWashDto.unitSenderName;
            productCode = serviceSubconFabricWashDto.productCode;
            productName = serviceSubconFabricWashDto.productName;
            productRemark = serviceSubconFabricWashDto.productRemark;
            designcolor = serviceSubconFabricWashDto.designcolor;
            quantity = serviceSubconFabricWashDto.quantity;
            uomUnit = serviceSubconFabricWashDto.uomUnit;
        }
    }
}
