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
    internal class OrganizerRatingRepository : BaseRepository<OrganizerRating, int>, IOrganizerRatingRepository
    {
        private readonly EventaDbContext _context;
        private readonly DbSet<OrganizerRating> _dbSet;

        public OrganizerRatingRepository(EventaDbContext context) : base(context)
        {
            _context = context;
            _dbSet = _context.Set<OrganizerRating>();
        }
    }
}
