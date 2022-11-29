using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentPreparings
{
    public class GarmentPreparingTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentPreparing item = new GarmentPreparing(new GarmentPreparingReadModel(id));
            item.setUENId(1);
            item.SetUnitId(new UnitDepartmentId(1));
            Assert.NotNull(item);
        }
    }
}
