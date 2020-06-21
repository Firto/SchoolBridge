using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;
using System.Linq;

namespace SchoolBridge.API.Controllers.Attributes.Globalization
{
    public class BaseUpatedIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("baseupdateid") || 
                context.HttpContext.RequestServices.GetService<ILanguageStringService>().GetUpdateId() != context.HttpContext.Request.Headers["baseupdateid"])
                throw new ClientException("lss-baseupateid-inc"); 
            else context.ActionArguments["baseUpdateId"] = context.HttpContext.Request.Headers["baseupdateid"].First();
        }
    }
}