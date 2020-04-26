using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace AN.Integration.API.Filters
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public AuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var requestToken = context.HttpContext.Request.Headers["Authorization"];
            var auth = requestToken == _configuration["IntegrationApi:AccessToken"];
            if (!auth)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}