using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetWIP
{
    public class GetWIPQuery : IQuery<GarmentWIPListViewModel>
    {
        public int page { get; private set; }
        public int size { get; private set; }
        public string order { get; private set; }
        public string token { get; private set; }
        public DateTime Date { get; private set; }

        public GetWIPQuery(int page, int size, string order, DateTime date, string token)
        {
            this.page = page;
            this.size = size;
            this.order = order;
            this.Date = date;
            this.token = token;
        }
    }
}
