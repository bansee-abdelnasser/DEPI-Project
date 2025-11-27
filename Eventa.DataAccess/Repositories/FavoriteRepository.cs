using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Repositories
{
    internal class FavoriteRepository : BaseRepository<UserFavorite, int>, IFavoriteRepository
    {
        private readonly EventaDbContext _context;
        private readonly DbSet<UserFavorite> _dbSet;

        public FavoriteRepository(EventaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<UserFavorite>();
        }

 

    }

}
