using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class BaseResult
    {
        public string apiVersion { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }
}
