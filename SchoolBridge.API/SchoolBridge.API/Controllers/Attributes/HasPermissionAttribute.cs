using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Helpers.Managers.CClientErrorManager.Middleware;

namespace SchoolBridge.API.Controllers.Attributes
{
    public class HasPermissionAttribute: MyAutorizeAttribute
    {
        private readonly string[] names = null;

        public HasPermissionAttribute(params string[] names)
        {
            this.names = names;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (names != null && 
                (
                    (names.Length == 1 && !context.HttpContext.RequestServices.GetService<IPermissionService>().HasPermission((User)context.ActionArguments["user"], names[0])) || 
                    (names.Length > 1 && !context.HttpContext.RequestServices.GetService<IPermissionService>().HasAllPermissions((User)context.ActionArguments["user"], names))
                )
            )
                throw new ClientException("no-access");
        }
    }
}
