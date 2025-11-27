using Eventa.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.DataContext
{
    internal class EventaDbContext : IdentityDbContext<AppUser>
    {
        public EventaDbContext(DbContextOptions<EventaDbContext> options) : base(options)
        {
        }

        public DbSet<UserFavorite> UserFavorites { get; set; }

    }
}
