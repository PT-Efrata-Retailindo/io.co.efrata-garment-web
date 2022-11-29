using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsDynamic
{
    public class GarmentSewingOutsDynamicViewModel
    {
        public int total { get; private set; }
        public List<dynamic> data { get; private set; }

        public GarmentSewingOutsDynamicViewModel(int total, List<dynamic> data)
        {
            this.total = total;
            this.data = data;
        }
    }
}
