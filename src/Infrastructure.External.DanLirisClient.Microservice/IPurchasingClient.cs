using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.Microservice
{
    public interface IPurchasingClient
    {
        Task<string> SetGarmentUnitExpenditureNote(int id);
    }
}
