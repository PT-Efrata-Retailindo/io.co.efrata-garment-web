using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Moonlay.Domain;
using Newtonsoft.Json;
using System.Linq;
using System.Net;

namespace Infrastructure.Mvc.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebApiContext _workContext;

        public GlobalExceptionFilter(IWebApiContext apiContext)
        {
            _workContext = apiContext;
        }

        public void OnException(ExceptionContext context)
        {
            StringValues contentTypes = new StringValues();
            context.HttpContext.Request.Headers.TryGetValue("Content-Type", out contentTypes);
            object info = null, data = null;

            // FOR API
            if (!contentTypes.Any(o => o == "application/x-www-form-urlencoded"))
            {
                HttpStatusCode status = HttpStatusCode.InternalServerError;
                string message = context.Exception.Message;

                // FOR API
                if (context.Exception is DomainNullException || context.Exception.InnerException is DomainNullException)
                {
                    DomainNullException ex;
                    if (context.Exception is DomainNullException)
                        ex = context.Exception as DomainNullException;
                    else
                        ex = context.Exception.InnerException as DomainNullException;

                    context.ModelState.AddModelError(ex.ParamName, ex.Message);

                    info = context.ModelState.Select(o => new { propertyName = o.Key, errors = o.Value.Errors.Select(e => e.ErrorMessage) });
                    message = ex.Message;
                    status = HttpStatusCode.BadRequest;
                }
                else if (context.Exception is FluentValidation.ValidationException || context.Exception.InnerException is FluentValidation.ValidationException)
                {
                    FluentValidation.ValidationException ex;
                    if (context.Exception is FluentValidation.ValidationException)
                        ex = context.Exception as FluentValidation.ValidationException;
                    else
                        ex = context.Exception.InnerException as FluentValidation.ValidationException;

                    foreach (var failures in ex.Errors)
                    {
                        context.ModelState.AddModelError(failures.PropertyName, failures.ErrorMessage);
                    }

                    info = context.ModelState.Select(o => new { propertyName = o.Key, errors = o.Value.Errors.Select(e => e.ErrorMessage) });
                    message = ex.Message;
                    status = HttpStatusCode.BadRequest;
                }

                context.ExceptionHandled = true;

                HttpResponse response = context.HttpContext.Response;
                response.StatusCode = (int)status;
                response.ContentType = "application/json";

                response.WriteAsync(JsonConvert.SerializeObject(new
                {
                    apiVersion = _workContext.ApiVersion,
                    success = false,
                    data,
                    info,
                    message,
                    error = message,
                    trace = context.Exception.StackTrace
                }));
            }

            // FOR MVC
        }
    }
}