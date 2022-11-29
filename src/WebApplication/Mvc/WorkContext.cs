using Infrastructure;
using System.Diagnostics;

namespace DanLiris.Admin.Web
{
    public class WorkContext : IWebApiContext
    {
        public WorkContext()
        {
            ApiVersion = FileVersionInfo.GetVersionInfo(typeof(Startup).Assembly.Location).FileVersion;
        }

        public string CurrentUser { get; internal set; }
        public string UserName { get; set; }
        public string ApiVersion { get; internal set; }
        public string Token { get; set; }
        public int TimezoneOffset { get; set; }
    }
}