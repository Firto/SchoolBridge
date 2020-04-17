using Microsoft.AspNetCore.Mvc.Filters;
using SchoolBridge.Helpers.Managers;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;

namespace SchoolBridge.API.Controllers.Attributes
{
    public class UUIDAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string uuid;

            if (!UUIDHandler.CheckUUID(context.HttpContext.Request.Headers, out uuid))
                throw new ClientException("no-uuid"); 
            else context.ActionArguments["uuid"] = uuid;
        }
    }
}