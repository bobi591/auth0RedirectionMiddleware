using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace Auth0RedirectionMiddleware
{
    /// <summary>
    /// This delegate should be pointing to a method that implements the logic
    /// that determines if the current user is authenticated.
    /// </summary>
    /// <param name="context"></param>
    /// <returns>Boolean (when false the user will be challenged)</returns>
    public delegate bool AuthDelegate(HttpContext context);

    /// <summary>
    /// Auth0 challenge redirection middleware.
    /// </summary>
    public class Auth0RedirectionMiddleware
    {   
        private readonly RequestDelegate _next;
        private readonly Auth0RedirectionMiddlewareOptions _options;
        private readonly AuthDelegate _authDelegate;
            
        public Auth0RedirectionMiddleware(RequestDelegate next, Auth0RedirectionMiddlewareOptions options, AuthDelegate authDelegate)
        {
            _next = next;
            _authDelegate = authDelegate;

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
                if(_authDelegate!=null)
                {
                    bool isEligible = _authDelegate.Invoke(context);

                    if(!isEligible)
                    {
                        //Redirect to the Auth0 challenge.
                        await context.ChallengeAsync(_options.Scheme,
                            new AuthenticationProperties() { RedirectUri = _options.RedirectUri });
                        return;
                    }
                }
                else
                {
                    throw new Auth0RedirectionMiddlewareException("Authentication checks failure. The AuthDelegate is null.");
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
