
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.ApplicationInsights.AspNetCore
{
    public static class ApplicationInsightExtensions
    {
        public static IServiceCollection AddAppInsightRequestBodyLogging(this IServiceCollection services) => 
            services.AddTransient<RequestBodyLoggingMiddleware>().AddTransient<ResponseBodyLoggingMiddleware>();

        public static IApplicationBuilder UseAppInsightRequestBodyLogging(this IApplicationBuilder builder) => 
            builder.UseMiddleware<RequestBodyLoggingMiddleware>();

        public static IApplicationBuilder UseAppInsightResponseBodyLogging(this IApplicationBuilder builder) => 
            builder.UseMiddleware<ResponseBodyLoggingMiddleware>();
    }
}