using System.Linq;
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
        
        public async override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            base.OnAuthorizationAsync(context);

            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is

            if (hasAllowAnonymous)
            {
                return;
            }
            
            if (context.HttpContext.User.IsInRole("Administrator"))
            {
                return;
            }

            if (context.HttpContext.Request.Method == "POST")
            {
                context.Result = new ForbidResult("Bearer");
            }
        }
    }
}