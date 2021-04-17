using System;
namespace Auth0RedirectionMiddleware
{
    public class Auth0RedirectionMiddlewareOptions
    {
        public string RedirectUri { get; set; } = "/";
        public string Scheme { get; set; } = "Auth0";
    }
}
