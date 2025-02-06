using HousingManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HousingManagementAPI.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _table;

        public Repository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _table = _context.Set<T>();
        }

        public IQueryable<T> GetAll()
        {
            return _table;
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return _table.Where(filter);
        }

        public void Add(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _table.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _table.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _table.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
