using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity, TId> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllQueryable();
        TEntity? FindById(TId id);

        Task<TEntity?> FindByIdAsync(TId id);

        void Create(TEntity entity);

        Task CreateAsync(TEntity entity);
        void Update(TEntity entity);

        TEntity? Delete(TId id);
        Task<TEntity?> DeleteAsync(TId id);
    }
}
