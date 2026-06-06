using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(
       ActionExecutingContext context)
        {
            
        }

        public void OnActionExecuted(
       ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                throw context.Exception;
            }
        }
    }
}
