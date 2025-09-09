using ERestaurant.Application.Common.Interfaces;


namespace ERestaurant.Tests.Materials
{
    public partial class MaterialServiceTests
    {
        public class FakeCurrentUserService : ICurrentUserService
        {
            public Guid TenantId => Guid.NewGuid();
            public string? UserId => "test-user";
            public string? UserName => "UnitTestUser";
        }
    }
}
