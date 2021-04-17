using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace Auth0RedirectionMiddleware
{
    public class Auth0RedirectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Auth0RedirectionMiddlewareOptions _options;

        public Auth0RedirectionMiddleware(RequestDelegate next, Auth0RedirectionMiddlewareOptions options)
        {
            _next = next;

            if (options == null)
            {
                throw new Auth0RedirectionMiddlewareException("options parameter should not be null.");
            }

            _options = options;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    //Redirect to the Auth0 challenge.
                    await context.ChallengeAsync(_options.Scheme,
                        new AuthenticationProperties() { RedirectUri = _options.RedirectUri });
                    return;
                }

                // Let all the previous actions in the pipeline to finish.
                await _next(context);
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
