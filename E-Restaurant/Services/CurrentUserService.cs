using ERestaurant.Application.Common.Interfaces;

namespace E_Restaurant.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Static default tenant for seeding and fallback
        private static readonly Guid DefaultTenantId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <summary>
        /// Gets the current TenantId from HttpContext.Items["TenantId"].
        /// Falls back to DefaultTenantId if not provided.
        /// </summary>
        public Guid TenantId
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;

                if (context?.Items.TryGetValue("TenantId", out var tenantObj) == true &&
                    Guid.TryParse(tenantObj?.ToString(), out var tenantId))
                {
                    return tenantId;
                }

                return DefaultTenantId;
            }
        }

        /// <summary>
        /// Gets the current username from claims or defaults to "System".
        /// </summary>
        public string UserName =>
            _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
    }

}
