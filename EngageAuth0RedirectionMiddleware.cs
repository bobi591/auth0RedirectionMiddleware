using System;
namespace Auth0RedirectionMiddleware
{
    /// <summary>
    /// Attribute that toggles the Auth0RedirectionMiddleware checks for the current controller method.
    /// </summary>
    public class EngageAuth0RedirectionMiddleware : Attribute
    {
        public bool IsEngaged { get; set; }
        public EngageAuth0RedirectionMiddleware(bool isEngaged = true)
        {
            IsEngaged = isEngaged;
        }
    }
}
