namespace App.Web.Api.Filters
{
    public class FilterMiddlewarePipeline
    {
        public void Configure(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Pipeline", "Middleware");

                await next();
            });
        }
    }
}
