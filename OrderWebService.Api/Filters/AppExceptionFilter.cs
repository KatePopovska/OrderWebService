
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OrderWebService.Contracts.Http.Errors;
using OrderWebService.Contracts.Http.Requests;

namespace OrderWebService.Api.Filters
{
    internal class AppExceptionFilter : IAsyncExceptionFilter
    {
        private ILogger<AppExceptionFilter> _logger;

        public AppExceptionFilter(ILogger<AppExceptionFilter> logger)
        {
            _logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Unhandled exception");

            context.Result = new ObjectResult(new ErrorResponse
            {
                Errors = new[] { context.Exception.Message }
            })

            {
                StatusCode = 500
            };

            return Task.CompletedTask;

        }
    }
}
