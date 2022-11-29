using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentExpenditureGoodReturns
{
    public class GarmentExpenditureGoodReturnTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentExpenditureGoodReturn item = new GarmentExpenditureGoodReturn(new GarmentExpenditureGoodReturnReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
