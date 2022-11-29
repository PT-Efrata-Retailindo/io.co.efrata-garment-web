using System.Threading.Tasks;

namespace Infrastructure.External.DanLirisClient.Microservice
{
    public interface ICoreClient
    {
        Task<dynamic> RetrieveUnitDepartments();
        void SetUnitDepartments();
        void SetProduct();
        //void SetMachineSpinning();
        void SetMachineType();
        void SetUoms();
        void SetGarmentProducts();
        void SetStorages();
        void SetGarmentComodities();
    }
}