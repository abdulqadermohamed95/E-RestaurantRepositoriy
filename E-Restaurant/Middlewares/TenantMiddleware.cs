namespace E_Restaurant.Middlewares
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantId))
            {
                if (Guid.TryParse(tenantId, out var parsedTenant))
                {
                    context.Items["TenantId"] = parsedTenant;
                }
            }

            await _next(context);
        }
    }

}
