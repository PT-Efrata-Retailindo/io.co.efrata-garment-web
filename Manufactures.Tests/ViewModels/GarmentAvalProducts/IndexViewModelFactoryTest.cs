using Manufactures.ViewModels.GarmentAvalProducts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.ViewModels.GarmentAvalProducts
{
    public class IndexViewModelFactoryTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            IndexViewModel viewModel = new IndexViewModel();
            Assert.NotNull(viewModel);
        }
    }
}
