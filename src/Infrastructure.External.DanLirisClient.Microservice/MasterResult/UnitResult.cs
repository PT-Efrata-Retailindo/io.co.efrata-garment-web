using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class UnitResult : BaseResult
    {
        public UnitResult()
        {
            data = new List<Unit>();
        }
        public IList<Unit> data { get; set; }
    }

    public class SingleUnitResult : BaseResult
    {
        public Unit data { get; set; }
    }

    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
