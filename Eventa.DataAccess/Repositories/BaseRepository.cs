using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Repositories
{
    internal class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId> where TEntity : class
    {
        private readonly EventaDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(EventaDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public IQueryable<TEntity> GetAllQueryable()
        {
            return _dbSet.AsQueryable();
        }

        public TEntity? FindById(TId id)
        {
            return _dbSet.Find(id);
        }

        public void Create(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public TEntity? Delete(TId id)
        {
            TEntity? entity = FindById(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return entity;
            }
            return null;
        }

        public async Task<TEntity?> DeleteAsync(TId id)
        {
            TEntity? entity = await FindByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return entity;
            }
            return null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity?> FindByIdAsync(TId id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(TEntity entity)
        {
           await _dbSet.AddAsync(entity);
        }
    }
}
