using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Warehouse.Web.Api.Infrastructure.Filters;

public class OperationCancelledExceptionFilter: ExceptionFilterAttribute
{
    private readonly ILogger logger;

    public OperationCancelledExceptionFilter(ILoggerFactory factory)
    {
        logger = factory.CreateLogger<OperationCancelledExceptionFilter>();
    }

    public override void OnException(ExceptionContext context) => OnExceptionHandler(context);
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnExceptionHandler(context);
        return Task.CompletedTask;
    }

    private void OnExceptionHandler(ExceptionContext context)
    {
        if (context.Exception is OperationCanceledException)
        {
            logger.LogInformation("Request was cancelled");
            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult(499); //  499 Client Closed Request
        }
    }

}
