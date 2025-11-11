using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.UnitOfWork
{
    internal class EventaUnitOfWork:IUnitOfWork
    {
        private readonly EventaDbContext _context;

        public EventaUnitOfWork(EventaDbContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
