using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSewingDOs
{
    public class GarmentSewingDOTest
    {

        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSewingDO item = new GarmentSewingDO(new GarmentSewingDOReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
