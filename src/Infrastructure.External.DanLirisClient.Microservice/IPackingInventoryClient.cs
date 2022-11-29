using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.Microservice
{
    public interface IPackingInventoryClient
    {
        Task<bool> SetIsSampleExpenditureGood(string invoiceNo, bool isGarmentSampleExpenditureGood);
    }
}
