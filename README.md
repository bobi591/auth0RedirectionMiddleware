# auth0RedirectionMiddleware
ASP.NET Core middleware that toggles the Auth0 challenge when the visitor is not authenticated.

This middleware is workaraound of the bad Authorize Attribute redirection to the login page when Auth0 authentication is used.
The current Auth0 issue where the login uri cannot be set in some cases results in invalid login redirection when the [Authorize] attribute is used.
Using this middleware the unauthenticated visitors are getting redirected to the Auth0 challenge.

The authorization checks should be implemented by the consumer of the library. The checks are passed to the middleware through boolean delegate.

The middleware is executed only for the Controller methods that are decorated with the EngageAuth0RedirectionMiddleware attribute.

You can additionally customize this for your specific needs!! It can be used as substitute of the [Authorize] attribute in other cases as well.

Your configure method should look like this:
```csharp
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            AuthDelegate authDelegate = new AuthDelegate(this.CustomAuth); //Here the delegate is assigned to the custom auth method.

            app.UseAuth0RedirectionMiddlewareExtensions(new Auth0RedirectionMiddlewareOptions
            {
                Scheme = "Auth0",
                RedirectUri = "/"
            }, authDelegate); //Adding the middleware to the pipeline.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public bool CustomAuth(HttpContext context) //This is the custom auth method implementation.
        {
            if(context.User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
```

Your controller methods should be decorated like this:

```csharp
    [Route("auth")]
    public class LoginController : Controller
    {
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Service is up!");
        }

        [Route("isAuthorized")]
        [EngageAuth0RedirectionMiddleware] //Here the method is decorated with the attributes that toggles the Auth0RedirectionMiddleware
        public async Task<IActionResult> IsAuthorized()
        {
            return Ok("You are authorized!");
        }
    }
```