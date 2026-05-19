namespace API.Extensions
{
    public static class SwaggerMiddlewareExtensions
    {
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                // Endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                // UI Customization
                options.DocumentTitle = "API Docs";
                options.RoutePrefix = "";
                options.DefaultModelsExpandDepth(-1); // Hide schema section
                options.DisplayRequestDuration();     // Show API response time
                options.EnableDeepLinking();          // Bookmarkable URLs
                options.EnableFilter();               // Search/filter endpoints

                // Collapse sections by default
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

                // Optional: show extensions
                options.ShowExtensions();

                // Optional: persist auth token
                options.EnablePersistAuthorization();
            });

            return app;
        }
    }
}
