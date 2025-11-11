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
        IQueryable<TEntity> GetAllQueryable();
        TEntity? FindById(TId id);

        void Create(TEntity entity);
        void Update(TEntity entity);
        TEntity? Delete(TId id);
    }
}
