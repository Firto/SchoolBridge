using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SchoolBridge.Helpers.Extentions
{
    public class ScopedHttpContext
    {
        public HttpContext HttpContext { get; set; }
    }

    public class ScopedHttpContextMiddleware
    {
        private readonly RequestDelegate _next;

        public ScopedHttpContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context, ScopedHttpContext scopedContext)
        {
            scopedContext.HttpContext = context;
            return _next(context);
        }
    }
}
