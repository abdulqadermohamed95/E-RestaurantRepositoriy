namespace ERestaurant.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid TenantId { get; }
        string? UserName { get; }
    }
}
