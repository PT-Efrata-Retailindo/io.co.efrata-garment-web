using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.AzureUtility
{
    public static class TimeStamp
    {
        private const string TIMESTAMP_FORMAT = "yyyyMMddHHmmssffff";
        public static string Generate(DateTime value)
        {
            return value.ToString(TIMESTAMP_FORMAT);
        }
    }
}
