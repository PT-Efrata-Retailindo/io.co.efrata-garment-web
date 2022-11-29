using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.Repositories
{
    //Enhance Jason Aug 2021
    public interface ISewingInHomeListViewRepository : IAggregateRepository<SewingInHomeListView, SewingInHomeListViewReadModel>
    {
        IQueryable<SewingInHomeListViewReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<SewingInHomeListViewReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}