using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Eventa.DataAccess.Repositories
{
    public class EventRepository : BaseRepository<Event, int>, IEventRepository
    {
        private readonly EventaDbContext _context;
        private readonly DbSet<Event> _dbSet;

        public EventRepository(EventaDbContext context) :base(context)
        {
            _context = context;
            _dbSet = _context.Set<Event>();
        }

        public IEnumerable<Event> GetAll() => _dbSet.ToList();

        public Event? GetById(int id) => _dbSet.Find(id);

        public void Add(Event e) => _dbSet.Add(e);

        public void Update(Event e) => _dbSet.Update(e);

        public void Delete(Event e) => _dbSet.Remove(e);

       
    }
}
//DbUpdateException DatabaseUpdateException = Microsoft.EntityFrameworkCore.DbUpdateException; 'An error occurred while saving the entity changes. See the inner exception for details.'
