using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingRelationTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSubconCuttingRelation item = new GarmentSubconCuttingRelation(new GarmentSubconCuttingRelationReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
