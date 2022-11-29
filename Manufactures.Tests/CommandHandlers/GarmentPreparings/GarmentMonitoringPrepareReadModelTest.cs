using Manufactures.Domain.GarmentPreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentPreparings
{
  public  class GarmentMonitoringPrepareReadModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GarmentMonitoringPrepareReadModel model = new GarmentMonitoringPrepareReadModel()
            {
                article= "article",
                buyerCode= "buyerCode",
                expenditure=1,
                aval=1,
                mainFabricExpenditure=1,
                nonMainFabricExpenditure=1,
                productCode= "productCode",
                receipt=1,
                remainQty=1,
                remark="remark",
               roAsal= "roAsal",
               roJob= "roJob",
               stock=1,
               uomUnit= "uomUnit"
            };

            Assert.Equal("article", model.article);
            Assert.Equal("productCode", model.productCode);
            Assert.Equal("buyerCode", model.buyerCode);
            Assert.Equal(1, model.expenditure);
            Assert.Equal(1, model.nonMainFabricExpenditure);
            Assert.Equal(1, model.aval);
            Assert.Equal(1, model.mainFabricExpenditure);
            Assert.Equal(1, model.nonMainFabricExpenditure);
            Assert.Equal(1, model.receipt);
            Assert.Equal(1, model.remainQty);
            Assert.Equal(1, model.stock);
            Assert.Equal("uomUnit", model.uomUnit);
        }
    }
}
