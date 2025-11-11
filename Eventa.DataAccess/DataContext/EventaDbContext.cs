using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.DataContext
{
    internal class EventaDbContext : DbContext
    {
        public EventaDbContext(DbContextOptions<EventaDbContext> options) : base(options)
        {
        }
    }
}
