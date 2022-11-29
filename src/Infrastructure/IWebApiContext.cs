using Moonlay.ExtCore.Mvc.Abstractions;

namespace Infrastructure
{
    public interface IWebApiContext : IWorkContext
    {
        string UserName { get; set; }
        string ApiVersion { get; }
        string Token { get; set; }
        int TimezoneOffset { get; set; }
    }
}