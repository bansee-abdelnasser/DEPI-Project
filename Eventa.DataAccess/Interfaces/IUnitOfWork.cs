using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Repositories;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Eventa.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        UserManager<AppUser> UserManager { get; }
        IOrganizerRatingRepository OrganizerRating { get; }
        public ITokenManager TokenManager { get; }

        IFavoriteRepository Favorites { get; }

        //IBaseRepository<Event, int> Events { get; }
        //void SaveChanges();

        Task SaveChangesAsync();    
        ICategoryRepository Categories { get; }
        IEventRepository Events { get; }
        IAnnouncementRepository Announcements { get; }

        int Save();
    }
}
