using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentScrapTransactions
{
    public class GarmentScrapTransactionItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentScrapTransactionItem item = new GarmentScrapTransactionItem(new GarmentScrapTransactionItemReadModel(id));
            Assert.NotNull(item);
        }
    }
}
