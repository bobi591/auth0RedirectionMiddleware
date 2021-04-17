# auth0RedirectionMiddleware
ASP.NET Core middleware that toggles the Auth0 challenge when the visitor is not authenticated

This middleware is created as workaraound of the bad Authorize Attribute redirection to the login page when Auth0 authentication is used.
The current Auth0 issue where the login uri cannot be set in some cases makes the Authorize Attribute to redirect to an invalid login page.
Using this middleware the unauthenticated visitors are getting redirected to the Auth0 challenge directly without the need of additional code.

You can additionally customize this for your specific needs!!

How to setup:
In the Configure method of your Startup.cs class add "app.UseAuth0RedirectionMiddlewareExtensions();".
NOTE THAT THIS SHOULD GO AFTER "app.UseAuthentication(); & app.UseAuthorization();" in the HTTP request pipeline.
Your configure method should look like this:

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            // Register the Authentication middleware

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuth0RedirectionMiddlewareExtensions(new Auth0RedirectionMiddlewareOptions
            {
                Scheme = "Auth0",
                RedirectUri = "/"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
