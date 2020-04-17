using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SchoolBridge.DataAccess.Entities;
using SchoolBridge.Domain.Services.Abstraction;
using System.IdentityModel.Tokens.Jwt;

namespace SchoolBridge.API.Controllers.Attributes
{
    public class MyAutorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            JwtSecurityToken token = context.HttpContext.RequestServices.GetService<ITokenService<User>>().ValidateToken(context.HttpContext);
            context.ActionArguments["token"] = token;
            context.ActionArguments["user"] = new User { Id = token.Subject };
        }
    }
}