using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Warehouse.Web.Api.Infrastructure;
public class ValidateModelStateAttribute : ActionFilterAttribute
{
    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                .SelectMany(v => v.Errors)
                .Select(v => v.ErrorMessage)
                .ToList();
            //.ToDictionary(t => t.RawValue);

            //var responseObj = new ValidationProblemDetails(errors);
            var responseObj = new
            {
                Message = "Bad Request. Validation failure",
                Errors = errors
            };

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = 400
            };
        }
    }
}
