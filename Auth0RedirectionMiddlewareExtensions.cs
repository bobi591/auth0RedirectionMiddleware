using System;
using Microsoft.AspNetCore.Builder;

namespace Auth0RedirectionMiddleware
{
    /// <summary>
    /// Middleware that redirects to the Auth0 login page if the user is not authorized.
    /// </summary>
    public static class Auth0RedirectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuth0RedirectionMiddlewareExtensions
            (this IApplicationBuilder builder, Auth0RedirectionMiddlewareOptions options)
        {
            return builder.UseMiddleware<Auth0RedirectionMiddleware>(options);
        }
    }
}
