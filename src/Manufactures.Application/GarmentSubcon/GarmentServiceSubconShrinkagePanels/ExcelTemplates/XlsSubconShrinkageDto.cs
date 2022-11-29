using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.ExcelTemplates
{
    public class XlsSubconShrinkageDto
    {
        public XlsSubconShrinkageDto()
        {
        }

        public string noBon { get; internal set; }
        public string code { get; internal set; }
        public string name { get; internal set; }
        public string design { get; internal set; }
        public decimal quantity { get; internal set; }
        public string satuan { get; internal set; }

        public XlsSubconShrinkageDto(XlsSubconShrinkageDto xlsgarmentservicesubconshrinkagepanelsdto)
        {
            noBon = xlsgarmentservicesubconshrinkagepanelsdto.noBon;
            code = xlsgarmentservicesubconshrinkagepanelsdto.code;
            name = xlsgarmentservicesubconshrinkagepanelsdto.name;
            design = xlsgarmentservicesubconshrinkagepanelsdto.design;
            quantity = xlsgarmentservicesubconshrinkagepanelsdto.quantity;
            satuan = xlsgarmentservicesubconshrinkagepanelsdto.satuan;
        }
    }
}
