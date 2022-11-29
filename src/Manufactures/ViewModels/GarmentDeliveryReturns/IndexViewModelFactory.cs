using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.ViewModels.GarmentDeliveryReturns
{
    public class IndexViewModelFactory
    {
        public IndexViewModel Create(IStorage storage)
        {
            return new IndexViewModel()
            {
            };
        }
    }
}