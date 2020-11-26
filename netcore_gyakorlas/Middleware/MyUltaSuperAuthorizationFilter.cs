using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace netcore_gyakorlas.Middleware
{
    public class MyUltaSuperAuthorizationFilter : AuthorizeFilter
    {

        public MyUltaSuperAuthorizationFilter(AuthorizationPolicy policy): base(policy) {}
        
        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            base.OnAuthorizationAsync(context);

            if (context.HttpContext.User.IsInRole("Administrator"))
            {
                return Task.CompletedTask;
            }

            if (context.HttpContext.Request.Method == "POST")
            {
                context.Result = new ForbidResult("Bearer");
            }

            return Task.CompletedTask;
        }
    }
}