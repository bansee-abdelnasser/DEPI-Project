using Eventa.Application.Contracts;
using Eventa.Application.DTOs.User;
using Eventa.DataAccess.Entities;
using Eventa.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventa.Application.Services
{
    internal class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unit;

        public FavoriteService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        private IFavoriteRepository Favorites => _unit.Favorites;



        public async Task<IEnumerable<Event>> GetUserFavoritesAsync(string userId)
        {
            return await Favorites
                .GetAllQueryable()
                .Include(f => f.User)
                .Include(f => f.Event)
                .Where(f => f.User.Id == userId)
                .Select(f => f.Event)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool?> ToggleFavoriteAsync(int EventId, string UserId)
        {
            // Load User and Event
            var user = await _unit.UserManager.FindByIdAsync(UserId);
            var ev = await _unit.Events.FindByIdAsync(EventId);

            if (user == null || ev == null)
                return null;

            // Check if favorite exists
            var favorite = await Favorites
                .GetAllQueryable()
                .Include(f => f.User)
                .Include(f => f.Event)
                .SingleOrDefaultAsync(f => f.User.Id == UserId && f.Event.Id == EventId);

            if (favorite != null)
            {
                // Already favorited → remove
                Favorites.Delete(favorite.Id);
                await _unit.SaveChangesAsync();
                return false;
            }
            else
            {
                // Not favorited → add
                favorite = new UserFavorite
                {
                    User = user,
                    Event = ev,
                    FavoritedAt = DateTime.UtcNow
                };
                await Favorites.CreateAsync(favorite);
                await _unit.SaveChangesAsync();
                return true;
            }

        }
    }
}
