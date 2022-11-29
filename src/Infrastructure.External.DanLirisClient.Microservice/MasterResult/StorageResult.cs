using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class StorageResult : BaseResult
    {
        public StorageResult()
        {
            data = new List<Storage>();
        }
        public IList<Storage> data { get; set; }
    }

    public class SingleStorageResult : BaseResult
    {
        public Storage date { get; set; }
    }

    public class Storage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}