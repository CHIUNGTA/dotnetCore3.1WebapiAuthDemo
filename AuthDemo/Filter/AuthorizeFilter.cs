using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthDemo.Filter
{
    public class AuthorizeFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}
