using ERestaurant.Application.Common.Interfaces;
using ERestaurant.Domain.Common;
using ERestaurant.Domain.Entities.Combos;
using ERestaurant.Domain.Entities.Materials;
using ERestaurant.Domain.Entities.Orders;
using ERestaurant.Infrastructure.configurations;
using Microsoft.EntityFrameworkCore;

namespace ERestaurant.Infrastructure.Persistence
{
    public class ERestaurantDbContext : DbContext
    {

        private readonly ICurrentUserService _currentUserService;

        public Guid CurrentTenantId => _currentUserService.TenantId;

        public ERestaurantDbContext(DbContextOptions<ERestaurantDbContext> options, ICurrentUserService currentUserService)
         : base(options)
        {
            _currentUserService = currentUserService;
        }

        #region Entities
        public DbSet<Material> Materials { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboMaterial> ComboMaterials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MaterialConfig());
            modelBuilder.ApplyConfiguration(new ComboConfig());
            modelBuilder.ApplyConfiguration(new ComboMaterialConfig());
            modelBuilder.ApplyConfiguration(new OrderConfig());
            modelBuilder.ApplyConfiguration(new OrderItemConfig());

            //modelBuilder.Entity<Order>()
            //   .HasQueryFilter(o => o.TenantId == _currentUserService.TenantId);

            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// savechanges for any action in DB
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.TenantId = _currentUserService.TenantId ;
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserName;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = _currentUserService.UserName;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedOn = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = _currentUserService.UserName;
                        break;
                }

                if (entry.Entity.IsDeleted && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
