using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace wms.infrastructure.Attributes
{
    public class ValidationResultAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext ctx)
        {
            if (!ctx.HttpContext.Items.TryGetValue("ValidationResult", out var value))
            {
                return;
            }
            if (!(value is ValidationResult vldResult))
            {
                return;
            }
            var model = new ErrorModel { ErrorMessage = string.Join("\r\n", vldResult.Errors.Select(err => err.ErrorMessage)) };
            ctx.Result = new BadRequestObjectResult(model);
        }
    }

    public class ErrorModel
    {
        public string ErrorMessage { get; set; }
    }
}
