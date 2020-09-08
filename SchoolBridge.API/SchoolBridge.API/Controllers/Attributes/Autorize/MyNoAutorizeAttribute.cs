using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using SchoolBridge.Domain.Managers.CClientErrorManager.Middleware;

namespace SchoolBridge.API.Controllers.Attributes
{
    public class MyNoAutorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                context.HttpContext.RequestServices.GetService<ITokenService>().GetUser(context.HttpContext);
                throw new ClientException("already-login");
            }
            catch (ClientException)
            {}   
        }
    }
}