using Eventa.DataAccess.DataContext;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Eventa.DataAccess.Repositories;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenManager _tokenManager;


        private readonly Lazy<IFavoriteRepository> _favorites;
        private readonly Lazy<IBaseRepository<Event, int>> _events;
        public EventaUnitOfWork(EventaDbContext context, UserManager<AppUser> userManger, ITokenManager tokenManager)
        {
            _context = context;
            _userManager = userManger;
            _tokenManager = tokenManager;
            _favorites = new Lazy<IFavoriteRepository>(() => new FavoriteRepository(_context));
            _events= new Lazy<IBaseRepository<Event, int>>(() => new BaseRepository<Event, int>(_context));


        }

        public UserManager<AppUser> UserManager => _userManager;
        public ITokenManager TokenManager => _tokenManager;

        
        public IFavoriteRepository Favorites => _favorites.Value;

        public IBaseRepository<Event, int> Events => _events.Value;

        public void SaveChanges()
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
