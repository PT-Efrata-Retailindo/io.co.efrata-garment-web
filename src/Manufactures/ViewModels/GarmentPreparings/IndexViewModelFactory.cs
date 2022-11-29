using ExtCore.Data.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.ViewModels.GarmentPreparings
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
