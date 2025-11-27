using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Repositories.Todo.DataAccess.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.DataAccess.Interfaces
{
    public interface IUnitOfWork
    {
        UserManager<AppUser> UserManager { get; }
        public ITokenManager TokenManager { get; }

        IFavoriteRepository Favorites { get; }

        IBaseRepository<Event, int> Events { get; }
        void SaveChanges();

        Task SaveChangesAsync();    
    }
}
