using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace wms.infrastructure.Exceptions
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            var activity = Activity.Current;
            while (activity != null)
            {
                activity.Dispose();
                activity = activity.Parent;
            }

            Console.Write($"Internal server error: {ex.Message} {ex.InnerException?.Message} {ex.StackTrace}");
            context.Result = new JsonResult($"Internal server error. {ex.Message} {ex.InnerException?.Message} {ex.StackTrace} ")
            {
                StatusCode = 500
            };
        }
        public void Dispose() { }
    }
}
