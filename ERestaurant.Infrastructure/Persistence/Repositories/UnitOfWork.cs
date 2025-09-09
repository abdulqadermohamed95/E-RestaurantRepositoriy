using ERestaurant.Application.Common.Interfaces;
using System.Collections.Concurrent;

namespace ERestaurant.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fildes
        private readonly ERestaurantDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;
        #endregion

        #region Constructor
        public UnitOfWork(ERestaurantDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }
        #endregion

        #region Methodes
        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repositories.Repository<T>(_context);
                _repositories.TryAdd(type, repositoryInstance);
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
