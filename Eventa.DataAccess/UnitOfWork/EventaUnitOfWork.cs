using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.UnitOfWork
{
    public class EventaUnitOfWork : IUnitOfWork
    {
        private readonly EventaDbContext _context;

        public ICategoryRepository Categories { get; }
        public IEventRepository Events { get; }
        public IAnnouncementRepository Announcements { get; }

        public EventaUnitOfWork(
         EventaDbContext context,
         IEventRepository events,
         ICategoryRepository categories,
         IAnnouncementRepository announcements
)
        {
            _context = context;
            Events = events;
            Categories = categories;
            Announcements = announcements;
        }


        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
