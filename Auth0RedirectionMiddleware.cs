using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace Auth0RedirectionMiddleware
{
    public class Auth0RedirectionMiddleware
    {
        private readonly RequestDelegate _next;

        public Auth0RedirectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    // Let all the previous actions in the pipeline to finish.
                    await _next(context);

                    //Redirect to the Auth0 challenge.
                    await context.ChallengeAsync("Auth0", new AuthenticationProperties() { RedirectUri = "" });
                }
            }
            catch(Exception e)
            {
                throw new Auth0RedirectionMiddlewareException(e.Message);
            }
        }
    }

    public class Auth0RedirectionMiddlewareException : Exception
    {
        public Auth0RedirectionMiddlewareException()
        {

        }

        public Auth0RedirectionMiddlewareException(string message)
            : base(message)
        {

        }

        public Auth0RedirectionMiddlewareException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
